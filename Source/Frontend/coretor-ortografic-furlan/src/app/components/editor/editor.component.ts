import { Component, OnInit } from '@angular/core';
import { SignalRService } from 'src/app/services/SignalR/SignalRService';
import Editor from 'ckeditor5-custom-build/build/ckeditor';

@Component({
  selector: 'app-editor',
  templateUrl: './editor.component.html',
  styleUrls: ['./editor.component.scss'],
})
export class EditorComponent implements OnInit {
  private wordsState: {
    [word: string]: { isCorrect: boolean; suggestions: string[] };
  } = {};
  private debounceTimer: any;

  public editor: any = Editor;
  public editorContent = '';

  public config = {
    placeholder: 'Tache a scrivi alc...',
    disableNativeSpellChecker: true
  };

  constructor(private signalRService: SignalRService) {}

  ngOnInit(): void {
    this.initializeSignalR();
  }

  initializeSignalR(): void {
    this.signalRService.startConnection();

    this.signalRService.registerSuggestionsCallback((result) => {
      this.updateWordState(result.word, result.isCorrect, result.suggestions);
    });

    this.signalRService.registerTextCheckCallback((result) => {
      this.handleTextCheckResult(result);
    });
  }

  onTextChange(): void {
    clearTimeout(this.debounceTimer);
    this.debounceTimer = setTimeout(() => {
      const cleanContent = this.cleanHtmlContent(this.editorContent);
      this.signalRService.checkText(cleanContent);
    }, 500);
  }

  private handleTextCheckResult(result: any): void {
    result.forEach((wordResult: any) => {
      this.updateWordState(wordResult.original, wordResult.correct, wordResult.suggestions);
    });
    this.updateEditorContent();
  }

  private checkLastWord(htmlContent: string): void {
    const text = this.extractAndCleanTextFromHTML(htmlContent);
    const words = text.trim().split(/\s+/);
    const lastWord = words[words.length - 1];
    if (lastWord) {
      this.signalRService.suggestWords(lastWord);
    }
  }

  private updateWordState(
    word: string,
    isCorrect: boolean,
    suggestions: string[] = []
  ): void {
    this.wordsState[word] = { isCorrect, suggestions };
    // Now, handle the suggestions. For example, you could display them in a tooltip or a dropdown.
    this.updateEditorContent();
  }

  private updateEditorContent(): void {
    let content = this.editorContent;

    for (const [word, state] of Object.entries(this.wordsState)) {
      if (!state.isCorrect) {
        // More complex regex to avoid already wrapped words
        const regex = new RegExp(
          `(?!<span[^>]*?>)\\b${word}\\b(?!<\/span>)`,
          'g'
        );

        if (regex.test(content)) {
          // Wrap the word if it's not already within a <span>
          const styledWord = `<span style="color:hsl(0, 75%, 60%);">${word}</span>`;
          content = content.replace(regex, styledWord);
          console.log(`Wrapping word '${word}' with style.`);
        } else {
          console.log(`Word '${word}' is already styled. No action taken.`);
        }
      }
    }

    if (this.editorContent !== content) {
      this.editorContent = content;
    }
  }

  private extractAndCleanTextFromHTML(htmlContent: string): string {
    const doc = new DOMParser().parseFromString(htmlContent, 'text/html');
    return doc.body.textContent || '';
  }

  private cleanHtmlContent(htmlContent: string): string {
    const doc = new DOMParser().parseFromString(htmlContent, 'text/html');
    let textContent = doc.body.textContent || '';

    // Replace HTML entities like &nbsp; with a space
    textContent = textContent.replace(/&nbsp;/g, ' ');

    return textContent;
  }
}
