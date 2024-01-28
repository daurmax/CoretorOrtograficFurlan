import { Component, ComponentRef, OnInit, ViewContainerRef } from '@angular/core';
import { SignalRService } from 'src/app/services/SignalR/SignalRService';
import { SuggestionsModalComponent } from '../suggestions-modal/suggestions-modal.component';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

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
  private destroy$ = new Subject<void>();

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

  constructor(
    private signalRService: SignalRService,
    private viewContainerRef: ViewContainerRef
  ) {}
  
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
        const word = node.textContent || '';
        this.currentWord = word;
        const suggestions = this.wordsState[word]?.suggestions ?? [];
        if (suggestions.length > 0) {
          this.showTooltip(word, suggestions, e);
        }
      } else if (this.currentTooltipRef) {
        // Close the tooltip if a non-incorrect-word is clicked
        this.currentTooltipRef.destroy();
        this.currentTooltipRef = null;
      }
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
      // Instead of setting the entire content, try to update only the changed parts
      this.applyContentUpdates(content);
  
      // Delay the restoration of the bookmark
      setTimeout(() => {
        this.editorInstance.selection.moveToBookmark(bookmark); 
        console.log("Bookmark Restored. Current Selection:", this.editorInstance.selection.getRng());
        this.editorInstance.focus(); 
      }, 25);
    }
  }

  private applyContentUpdates(updatedContent: string): void {
    const editorBody = this.editorInstance.getBody();
    editorBody.innerHTML = updatedContent;
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
    if (this.currentWord !== undefined) {
      const content = this.editorInstance.getBody();
      const spans = content.querySelectorAll('.incorrect-word');
  
      spans.forEach((span: Element) => {
        if (span.textContent === this.currentWord) {
          span.textContent = suggestion;
          span.classList.remove('incorrect-word');
          span.removeAttribute('style');
        }
      });
  
      // Update the wordsState
      this.wordsState[this.currentWord] = { isCorrect: true, suggestions: [] };
  
      // Close the tooltip
      if (this.currentTooltipRef) {
        this.currentTooltipRef.destroy();
        this.currentTooltipRef = null;
      }
  
      // Restore focus to the editor
      this.editorInstance.focus();
  
      // Optionally, you can also restore the cursor position to the end of the replaced word
      // You'll need to find the node where the replacement happened and set the cursor there
      const replacedNode = this.findNodeWithText(this.editorInstance.getBody(), suggestion);
      if (replacedNode) {
        this.editorInstance.selection.setCursorLocation(replacedNode, suggestion.length);
      }
    }
  }
  
  private findNodeWithText(node: Node, text: string): Node | null {
    const walker = document.createTreeWalker(node, NodeFilter.SHOW_TEXT, null);
    let currentNode = walker.nextNode();
    while (currentNode) {
      if (currentNode.nodeValue === text) {
        return currentNode;
      }
      currentNode = walker.nextNode();
    }
    return null;
  }
}
