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
  public editorContent = '<p>Hello, world!</p>';

  constructor(private signalRService: SignalRService) {}

  private debounceTimer: any;

  ngOnInit(): void {
    this.initializeSignalR();
  }

  initializeSignalR(): void {
    this.signalRService.startConnection();

    this.signalRService.registerWordCheckCallback((result) => {
      console.log(result);
      // Handle the result for word check
    });

    this.signalRService.registerSuggestionsCallback((suggestions) => {
      console.log(suggestions);
      // Handle the suggestions
    });
  }

  onTextChange(): void {
    clearTimeout(this.debounceTimer);
    this.debounceTimer = setTimeout(() => {
      this.checkLastWord(this.editorContent);
    }, 500);
  }

  private checkLastWord(htmlContent: string): void {
    const text = this.extractAndCleanTextFromHTML(htmlContent);
    const words = text.trim().split(/\s+/);
    const lastWord = words[words.length - 1];
    if (lastWord) {
      this.signalRService.checkWord(lastWord);
    }
  }

  private extractAndCleanTextFromHTML(htmlContent: string): string {
    const doc = new DOMParser().parseFromString(htmlContent, 'text/html');
    let text = doc.body.textContent || '';
    // Remove punctuation using regular expression
    text = text.replace(/[.,\/#!$%\^&\*;:{}=\-_`~()]/g,"");
    return text;
  }
}
