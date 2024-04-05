import { Component, ComponentRef, OnInit, ViewContainerRef } from '@angular/core';
import { SignalRService } from 'src/app/services/SignalR/SignalRService';
import { SuggestionsModalComponent } from '../suggestions-modal/suggestions-modal.component';
import { TranslateService } from '@ngx-translate/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { secretConfig } from 'src/config/secret-config';

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
  private currentTooltipRef: ComponentRef<SuggestionsModalComponent> | null = null;

  private currentWord: string | undefined;
  private currentWordNode: Element | null = null;
  private destroy$ = new Subject<void>();

  public tinyMCEApiKey = secretConfig.tinyMCEApiKey;
  public editorInstance: any;
  public editorContent = '';

  public config: any = {
    // TinyMCE configuration options
    placeholder: 'Tache a scrivi alc...',
    plugins: 'lists link image',
    menubar: false,
    statusbar: false,
    toolbar: 'undo redo | mycopy',
    setup: function(editor: any) {
      editor.ui.registry.addButton('mycopy', {
        text: 'Copie',
        onAction: function() {
          editor.execCommand('Copy');
        }
      });
    }
  };

  constructor(
    private signalRService: SignalRService,
    private viewContainerRef: ViewContainerRef,
    private translate: TranslateService
  ) {
    translate.setDefaultLang('fur');
  }

  switchLanguage(language: string) {
    this.translate.use(language);
  }

  ngOnInit(): void {
    this.signalRService.onConnected(() => {
      this.registerSignalRCallbacks();
    });
    this.signalRService.startConnection();

    // Listen for global clicks
    document.addEventListener('click', this.handleGlobalClick.bind(this), true);
  }

  ngOnDestroy(): void {
    // Emit to all subscribers that the component is being destroyed
    this.destroy$.next();
    this.destroy$.complete();

    // Unsubscribe from global click listener
    document.removeEventListener('click', this.handleGlobalClick.bind(this), true);
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

    this.editorInstance.on('click', (e: MouseEvent) => {
      const node = this.editorInstance.selection.getNode();
      if (node && node.className === 'incorrect-word') {
        this.currentWordNode = node;
        const word = node.textContent || '';
        if (word) {
          this.currentWord = word;
          const suggestions = this.wordsState[word]?.suggestions ?? [];
          if (suggestions.length > 0) {
            this.showTooltip(word, suggestions, e);
          }
        } else {
          this.currentWord = undefined;
        }
      } else {
        this.currentWordNode = null;
        this.currentWord = undefined;
        if (this.currentTooltipRef) {
          this.currentTooltipRef.destroy();
          this.currentTooltipRef = null;
        }
      }
    });

    this.editorInstance.on('keydown', (e: KeyboardEvent) => {
      if (e.key === ' ') {
        const node = this.editorInstance.selection.getNode();
        if (node && node.className === 'incorrect-word') {
          const range = this.editorInstance.selection.getRng();
          if (range && range.startOffset === node.textContent.length) {
            e.preventDefault(); // Prevent the default space insertion
            this.insertSpaceAfterIncorrectWord();
          }
        }
        // If not in an incorrect word, the default space behavior will occur
      }
    });
  }

  private insertSpaceAfterIncorrectWord(): void {
    const node = this.editorInstance.selection.getNode();
    if (node && node.className === 'incorrect-word') {
      const range = this.editorInstance.selection.getRng();
      if (range && range.startOffset === node.textContent.length) {
        // Move the cursor outside the span and insert a space
        const newRange = document.createRange();
        newRange.setStartAfter(node);
        newRange.setEndAfter(node);
        this.editorInstance.selection.setRng(newRange);
        this.editorInstance.insertContent(' '); // Insert a normal space
      }
    }
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
    let content = this.editorContent;

    // Flag to check if content was updated
    let contentUpdated = false;

    for (const [word, state] of Object.entries(this.wordsState)) {
      if (!state.isCorrect) {
        const regex = new RegExp(
          `(?!<span[^>]*?class="incorrect-word"[^>]*?>)\\b${word}\\b(?!<\/span>)`,
          'g'
        );
        const styledWord = `<span class="incorrect-word" style="color: rgb(224, 62, 45); text-decoration: underline;" data-mce-style="color: rgb(224, 62, 45); text-decoration: underline;">${word}</span>`;
        if (content.search(regex) !== -1) {
          content = content.replace(regex, styledWord);
          contentUpdated = true;
        }
      }
    }

    if (contentUpdated) {
      // Save the current selection
      const bookmark = this.editorInstance.selection.getBookmark(2, true);
      console.log("Initial Bookmark:", bookmark);

      bookmark.start[0]++
      bookmark.start[1]++
      bookmark.start[2]++

      // Update the content
      this.editorInstance.setContent(content);

      // Restore the selection
      this.editorInstance.selection.moveToBookmark(bookmark);

      const newRange = this.editorInstance.selection.getRng();
      console.log("New Range:", newRange);
    }
  }

  private cleanHtmlContent(htmlContent: string): string {
    // Cleans HTML content to get plain text
    // This method might need adjustments based on how TinyMCE handles content
    const doc = new DOMParser().parseFromString(htmlContent, 'text/html');
    return doc.body.textContent || '';
  }

  private showTooltip(word: string, suggestions: string[], event: MouseEvent): void {
    // Check if there's an existing tooltip and remove it
    if (this.currentTooltipRef) {
      this.currentTooltipRef.destroy();
      this.currentTooltipRef = null;
    }

    // Create the new tooltip component
    this.currentTooltipRef = this.viewContainerRef.createComponent(SuggestionsModalComponent);

    // Assign the necessary inputs
    this.currentTooltipRef.instance.word = word;
    this.currentTooltipRef.instance.suggestions = suggestions;

    // Subscribe to the suggestionSelected event
    this.currentTooltipRef.instance.suggestionSelected.pipe(
      takeUntil(this.destroy$)
    ).subscribe((selectedSuggestion: string) => {
      this.onSuggestionSelect(selectedSuggestion);
    });

    // Get the position of the selected word in the editor
    const rect = this.editorInstance.selection.getRng().getBoundingClientRect();
    const editorPosition = this.editorInstance.getContainer().getBoundingClientRect();

    // Calculate the position for the tooltip
    const position = {
      x: rect.left - editorPosition.left,
      y: rect.bottom - editorPosition.top
    };

    // Position the tooltip component
    const tooltipElement = this.currentTooltipRef.location.nativeElement;
    tooltipElement.style.position = 'absolute';
    tooltipElement.style.left = `${position.x}px`;
    tooltipElement.style.top = `${position.y + 110}px`; // position below the word
  }

  private handleGlobalClick(event: MouseEvent): void {
    // Logic to determine if the click is outside the tooltip
    if (this.currentTooltipRef && !this.currentTooltipRef.location.nativeElement.contains(event.target)) {
      this.currentTooltipRef.destroy();
      this.currentTooltipRef = null;
    }
  }

  onSuggestionSelect(suggestion: string): void {
    if (this.currentWordNode && this.currentWord !== undefined) {
      // Replace the text of the node with the suggestion
      this.currentWordNode.textContent = suggestion;
      this.currentWordNode.classList.remove('incorrect-word');
      this.currentWordNode.removeAttribute('style');

      // Update the wordsState
      this.wordsState[this.currentWord] = { isCorrect: true, suggestions: [] };

      // Close the tooltip
      if (this.currentTooltipRef) {
        this.currentTooltipRef.destroy();
        this.currentTooltipRef = null;
      }

      // Restore focus to the editor
      this.editorInstance.focus();

      // Using TinyMCE API to set the cursor position at the end of the replaced word
      this.editorInstance.selection.setCursorLocation(this.currentWordNode, this.currentWordNode.childNodes.length);
    }
  }
}
