import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-suggestions-modal',
  templateUrl: './suggestions-modal.component.html',
  styleUrls: ['./suggestions-modal.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SuggestionsModalComponent implements OnInit {
  @Input() word: string | undefined;
  @Input() suggestions: string[] | undefined;

  ngOnInit(): void {
    console.log('Word:', this.word);
    console.log('Suggestions:', this.suggestions);
  }
}
