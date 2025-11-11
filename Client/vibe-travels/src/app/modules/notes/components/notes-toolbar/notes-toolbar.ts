import { Component, output } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-notes-toolbar',
  imports: [MatToolbarModule, MatButtonModule, MatIconModule],
  templateUrl: './notes-toolbar.html',
  styleUrl: './notes-toolbar.scss'
})
export class NotesToolbar {
  addNoteClick = output<void>();

  onAddNote(): void {
    this.addNoteClick.emit();
  }
}
