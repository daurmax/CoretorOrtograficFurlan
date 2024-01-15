import { Component, OnInit } from '@angular/core';
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { SignalRService } from 'src/app/services/SignalR/SignalRService';

@Component({
  selector: 'app-editor',
  templateUrl: './editor.component.html',
  styleUrls: ['./editor.component.css']
})
export class EditorComponent implements OnInit {
  public Editor = ClassicEditor;
  public editorContent: string | null = null;

  constructor(private signalRService: SignalRService) { }

  ngOnInit(): void {
    this.editorContent = "<p>Type here...</p>";
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
    const text = event.editor.getData();
    // TODO: Extract and send individual words to the SignalR service for checking
    // You might want to throttle or debounce this action to avoid sending too many requests
  }
}