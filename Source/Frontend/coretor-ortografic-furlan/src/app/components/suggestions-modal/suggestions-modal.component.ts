import { Component, Input, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-suggestions-modal',
  templateUrl: './suggestions-modal.component.html',
  styleUrls: ['./suggestions-modal.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SuggestionsModalComponent {
  @Input() word: string | undefined;
  @Input() suggestions: string[] | undefined;
}
