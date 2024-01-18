import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection: signalR.HubConnection | null = null;
  private readonly reconnectInterval = 5000; // Time in milliseconds to wait before attempting reconnection

  public startConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.API_BASE_PATH}/spellcheckhub`)
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => {
        console.error('Error while starting connection: ', err);
        this.handleStartConnectionError(err);
      });

    this.hubConnection.onclose(err => {
      console.error('Connection closed: ', err);
      this.handleConnectionClose(err);
    });
  }

  private handleStartConnectionError(error: Error): void {
    console.error('Start connection error: ', error);

    this.attemptReconnect();
  }

  private handleConnectionClose(error: Error | undefined): void {
    console.error('Connection closed by the server: ', error);

    this.attemptReconnect();
  }

  private attemptReconnect(): void {
    setTimeout(() => {
      console.log('Attempting reconnection...');
      this.startConnection();
    }, this.reconnectInterval);
  }

  public registerWordCheckCallback(callback: (result: any) => void): void {
    this.hubConnection?.on('ReceiveCheckWordResult', (data) => {
      callback(data);
    });
  }

  public registerSuggestionsCallback(callback: (suggestions: any) => void): void {
    this.hubConnection?.on('ReceiveSuggestWordsResult', (data) => {
      callback(data);
    });
  }

  public checkWord(word: string): void {
    this.hubConnection?.invoke('CheckWord', word)
      .catch(err => console.error(err));
  }

  public suggestWords(word: string): void {
    this.hubConnection?.invoke('SuggestWords', word)
      .catch(err => console.error(err));
  }
}