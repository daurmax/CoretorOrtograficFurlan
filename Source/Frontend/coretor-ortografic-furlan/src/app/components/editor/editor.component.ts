import { Component, OnInit } from '@angular/core';
import { SignalRService } from 'src/app/services/SignalR/SignalRService';
import TinyMCEEditor from 'tinymce';

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

  public editorInstance: any;
  public editorContent = '';

  public config: any = {
    // TinyMCE configuration options
    placeholder: 'Tache a scrivi alc...',
    plugins: 'lists link image',
    toolbar:
      'undo redo | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image',
    // Add other TinyMCE specific configurations as needed
  };

  constructor(private signalRService: SignalRService) {}

  ngOnInit(): void {
    this.signalRService.onConnected(() => {
      this.registerSignalRCallbacks();
    });
    this.signalRService.startConnection();
  }

  initializeSignalR(): void {
    this.signalRService.onConnected(() => {
      this.registerSignalRCallbacks();
    });
  }

  private registerSignalRCallbacks(): void {
    this.signalRService.registerSuggestionsCallback((result) => {
      this.updateWordState(result.word, result.isCorrect, result.suggestions);
    });

    this.signalRService.registerTextCheckCallback((result) => {
      this.handleTextCheckResult(result);
    });
  }

  onEditorInit(event: { editor: any }): void {
    this.editorInstance = event.editor;

    // Add an event listener for NodeChange
    this.editorInstance.on('NodeChange', () => {
      this.logCursorPosition();
    });
  }

  private logCursorPosition(): void {
    const selection = this.editorInstance.selection;
    const range = selection.getRng(); // Get the current range

    // Log the start and end positions of the range
    console.log(
      `Cursor Start Position: ${range.startOffset}, Cursor End Position: ${range.endOffset}`
    );
  }

  onTextChange(): void {
    clearTimeout(this.debounceTimer);
    this.debounceTimer = setTimeout(() => {
      const cleanContent = this.cleanHtmlContent(this.editorContent);
      this.signalRService.checkText(cleanContent);
    }, 500);
  }

  private handleTextCheckResult(result: any): void {
    // Processing the text check result
    result.forEach((wordResult: any) => {
      this.updateWordState(
        wordResult.original,
        wordResult.correct,
        wordResult.suggestions
      );
    });
    this.updateEditorContent();
  }

  private updateWordState(
    word: string,
    isCorrect: boolean,
    suggestions: string[] = []
  ): void {
    // Update the state of each word
    this.wordsState[word] = { isCorrect, suggestions };
    this.updateEditorContent();
  }

  private updateEditorContent(): void {
    const contentBeforeUpdate = this.editorInstance.getContent();
    const bookmark = this.editorInstance.selection.getBookmark(2, true); // Save the cursor position
    bookmark.start[0] = bookmark.start[0] + 1; // Adjust the start position to avoid the <p> tag
    console.log("Initial Bookmark:", bookmark);

    let content = this.editorContent;

    for (const [word, state] of Object.entries(this.wordsState)) {
      if (!state.isCorrect) {
        const regex = new RegExp(
          `(?!<span[^>]*?class="incorrect-word"[^>]*?>)\\b${word}\\b(?!<\/span>)`,
          'g'
        );
        const styledWord = `<span class="incorrect-word" style="color: rgb(224, 62, 45); text-decoration: underline;" data-mce-style="color: rgb(224, 62, 45); text-decoration: underline;">${word}</span>`;
        content = content.replace(regex, styledWord);
      }
    }

    if (this.editorContent !== content) {
      this.editorInstance.setContent(content); // This one moves the caret at the beginning of the editor
      this.editorInstance.selection.moveToBookmark(bookmark); // Restore the cursor position
      console.log("Bookmark Restored. Current Selection:", this.editorInstance.selection.getRng());

      const newBookmark = this.editorInstance.selection.getBookmark(2, true);
      console.log("New Bookmark after Content Update:", newBookmark);

      this.editorInstance.focus(); // Focus the editor
    }
  }

  private setCursorToEnd(): void {
    const editor = this.editorInstance;
    editor.selection.select(editor.getBody(), true); // Select the entire content
    editor.selection.collapse(false); // Collapse selection to the end
    editor.focus(); // Focus the editor
  }

  private cleanHtmlContent(htmlContent: string): string {
    // Cleans HTML content to get plain text
    // This method might need adjustments based on how TinyMCE handles content
    const doc = new DOMParser().parseFromString(htmlContent, 'text/html');
    return doc.body.textContent || '';
  }
}
