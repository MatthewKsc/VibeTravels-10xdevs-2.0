import { Component, input, output } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { Note } from '../../models/note.model';

@Component({
  selector: 'app-notes-list',
  imports: [MatCardModule, MatIconModule, MatButtonModule],
  templateUrl: './notes-list.html',
  styleUrl: './notes-list.scss'
})
export class NotesList {
  notes = input.required<Note[]>();
  noteViewClick = output<string>();
  noteEditClick = output<string>();
  noteDeleteClick = output<string>();

  onView(id: string): void {
    this.noteViewClick.emit(id);
  }

  onEdit(id: string): void {
    this.noteEditClick.emit(id);
  }

  onDelete(id: string): void {
    this.noteDeleteClick.emit(id);
  }

  getContentPreview(content: string): string {
    return content.length > 100 ? content.substring(0, 100) + '...' : content;
  }
}
