import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection: HubConnection | null = null;

  public startConnection = () => {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${environment.API_BASE_PATH}/spellcheckhub`)
      .build();
  
    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.error('Error while starting connection: ' + err));
  };

  public checkWord(word: string) {
    this.hubConnection!.invoke('CheckWord', word)
      .catch(err => console.error(err));
  }

  public registerWordCheckCallback(callback: (result: any) => void) {
    this.hubConnection!.on('ReceiveCheckWordResult', data => {
      callback(data);
    });
  }

  public suggestWords(word: string) {
    this.hubConnection!.invoke('SuggestWords', word)
      .catch(err => console.error(err));
  }

  public registerSuggestionsCallback(callback: (suggestions: any) => void) {
    this.hubConnection!.on('ReceiveSuggestWordsResult', suggestions => {
      callback(suggestions);
    });
  }
}