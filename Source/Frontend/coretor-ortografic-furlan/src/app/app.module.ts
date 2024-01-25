import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { EditorComponent } from './components/editor/editor.component';
import { SuggestionsModalComponent } from './components/suggestions-modal/suggestions-modal.component';

import { FormsModule } from '@angular/forms';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { EditorModule } from '@tinymce/tinymce-angular';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

@NgModule({
  declarations: [
    AppComponent,
    EditorComponent,
    SuggestionsModalComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    CKEditorModule,
    EditorModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
