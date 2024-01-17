import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection: signalR.HubConnection | null = null;

  public startConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.API_BASE_PATH}/spellcheckhub`)
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err));
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