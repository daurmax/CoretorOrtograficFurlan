import { Component, OnInit } from '@angular/core';
import { SignalRService } from 'src/app/services/SignalR/SignalRService';
import Editor from 'ckeditor5-custom-build/build/ckeditor';

@Component({
  selector: 'app-editor',
  templateUrl: './editor.component.html',
  styleUrls: ['./editor.component.scss'],
})
export class EditorComponent implements OnInit {
  private wordsState: { [word: string]: { isCorrect: boolean, suggestions: string[] } } = {};
  private debounceTimer: any;

  public editor: any = Editor;
  public editorContent = '<p>Hello, world!</p>';

  constructor(private signalRService: SignalRService) {}

  ngOnInit(): void {
    this.initializeSignalR();
  }

  initializeSignalR(): void {
    this.signalRService.startConnection();

    this.signalRService.registerSuggestionsCallback((result) => {
      this.updateWordState(result.word, result.isCorrect, result.suggestions);
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
      this.signalRService.suggestWords(lastWord);
    }
  }

  private updateWordState(word: string, isCorrect: boolean, suggestions: string[] = []): void {
    this.wordsState[word] = { isCorrect, suggestions };
    this.updateEditorContent();
  }

  private updateEditorContent(): void {
    let content = this.editorContent;
  
    for (const [word, state] of Object.entries(this.wordsState)) {
      if (!state.isCorrect) {
        // Create a regex to check if the word is already wrapped in a span with the style
        const styledWordRegex = new RegExp(`<span style="color:hsl\\(0, 75%, 60%\\);">${word}</span>`, 'g');
  
        // Check if the word is already wrapped
        if (!styledWordRegex.test(content)) {
          // If not, wrap it
          const styledWord = `<span style="color:hsl(0, 75%, 60%);">${word}</span>`;
          content = content.replace(new RegExp(`\\b${word}\\b`, 'g'), styledWord);
  
          // Log the action
          console.log(`Wrapping word '${word}' with style.`);
        } else {
          // Log that the word is already wrapped
          console.log(`Word '${word}' is already styled. No action taken.`);
        }
      }
    }
  
    this.editorContent = content;
  }
  
  private extractAndCleanTextFromHTML(htmlContent: string): string {
    const doc = new DOMParser().parseFromString(htmlContent, 'text/html');
    return doc.body.textContent || '';
  }
}