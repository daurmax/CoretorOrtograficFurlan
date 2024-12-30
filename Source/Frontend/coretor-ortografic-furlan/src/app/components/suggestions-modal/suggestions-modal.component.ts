import { Component, EventEmitter, Input, OnInit, Output, ViewEncapsulation } from '@angular/core';

@Component({
    selector: 'app-suggestions-modal',
    templateUrl: './suggestions-modal.component.html',
    styleUrls: ['./suggestions-modal.component.scss'],
    encapsulation: ViewEncapsulation.None,
    standalone: false
})
export class SuggestionsModalComponent implements OnInit {
  @Input() word: string | undefined;
  @Input() suggestions: string[] | undefined;
  @Output() suggestionSelected = new EventEmitter<string>();

  ngOnInit(): void {}

  onSuggestionClick(suggestion: string): void {
    this.suggestionSelected.emit(suggestion);
  }
}
