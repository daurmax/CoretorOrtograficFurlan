import { Injectable, OnDestroy } from '@angular/core';
import { BehaviorSubject, Subject, Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap, takeUntil } from 'rxjs/operators';
import { SignalRService } from 'src/app/services/SignalR/SignalRService';

/**
 * SpellCheckService:
 * - Listens for text changes
 * - Debounces and calls the server for spell-check
 * - Exposes a BehaviorSubject (wordsState$) that EditorComponent can subscribe to
 */
@Injectable({
  providedIn: 'root'
})
export class SpellCheckService implements OnDestroy {
  private destroy$ = new Subject<void>();

  /**
   * textSubject will receive new text whenever the user types in the editor.
   */
  private textSubject = new BehaviorSubject<string>('');

  /**
   * wordsState$ is a BehaviorSubject that emits the updated word-state
   * whenever the server returns new suggestions or corrections.
   */
  wordsState$ = new BehaviorSubject<{ 
    [word: string]: { isCorrect: boolean; suggestions: string[] } 
  }>({});

  constructor(private signalRService: SignalRService) {
    // On connect, register callbacks (or do it once in your main app flow)
    this.signalRService.onConnected(() => {
      this.registerSignalRCallbacks();
    });
    this.signalRService.startConnection();

    // Listen for changes to the text and handle them with a debounce
    this.textSubject.pipe(
      debounceTime(300),           // Adjust as needed
      distinctUntilChanged(),
      switchMap((text) => this.checkText(text)),  // Call the backend
      takeUntil(this.destroy$)
    ).subscribe((results) => {
      // Convert server response to a dictionary of { [word]: {isCorrect, suggestions} }
      const newWordsState: { [word: string]: { isCorrect: boolean; suggestions: string[] } } = {};
      results.forEach((wordResult: any) => {
        newWordsState[wordResult.original] = {
          isCorrect: wordResult.correct,
          suggestions: wordResult.suggestions,
        };
      });
      this.wordsState$.next(newWordsState);
    });
  }

  /**
   * Public method for components to pass new text in.
   */
  public updateText(text: string) {
    this.textSubject.next(text);
  }

  /**
   * Actually calls the server (through the SignalRService) to check text.
   * Adjust to match your actual server method signature.
   */
  private checkText(text: string): Observable<any> {
    return this.signalRService.checkText$(text);
  }

  /**
   * Register any additional callbacks (like single-word suggestions)
   * that come in from the server.
   */
  private registerSignalRCallbacks(): void {
    this.signalRService.registerSuggestionsCallback((result) => {
      // If you get a single-word suggestion result, you might merge it into wordsState
      const currentState = this.wordsState$.value;
      currentState[result.word] = {
        isCorrect: result.isCorrect,
        suggestions: result.suggestions
      };
      this.wordsState$.next({ ...currentState });
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
