import { Component, OnInit } from '@angular/core';
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { SignalRService } from 'src/app/services/SignalR/SignalRService';

@Component({
  selector: 'app-editor',
  templateUrl: './editor.component.html',
  styleUrls: ['./editor.component.scss'],
})
export class EditorComponent implements OnInit {
  public Editor = ClassicEditor;
  public editorContent = {
    editorData: '<p>Hello, world!</p>',
  };

  constructor(private signalRService: SignalRService) {}

  ngOnInit(): void {
    this.initializeSignalR();
  }

  initializeSignalR(): void {
    this.signalRService.startConnection();

    this.signalRService.registerWordCheckCallback((result) => {
      // TODO: Handle the result for word check
      // This is where you handle the response from the server
    });

    this.signalRService.registerSuggestionsCallback((suggestions) => {
      // TODO: Handle the suggestions
      // This is where you handle the suggestions response from the server
    });
  }

  onTextChange(event: any): void {
    const text = event.editorData;
    // TODO: Extract and send individual words to the SignalR service for checking
    // You might want to throttle or debounce this action to avoid sending too many requests
  }
}
