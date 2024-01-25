import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SuggestionsModalComponent } from './suggestions-modal.component';

describe('SuggestionsModalComponent', () => {
  let component: SuggestionsModalComponent;
  let fixture: ComponentFixture<SuggestionsModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SuggestionsModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SuggestionsModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
