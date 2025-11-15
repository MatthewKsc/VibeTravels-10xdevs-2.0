import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../../environments/environments';
import { Note, NoteFormData } from '../models/note.model';

@Injectable({
  providedIn: 'root'
})
export class NotesApiService {
  private httpClient = inject(HttpClient);
  private readonly noteApiUrl = environment.apiUrl + '/notes';

  getNotes(): Observable<Note[]> {
    return this.httpClient.get<Note[]>(this.noteApiUrl);
  }

  addNote(requestData: NoteFormData): Observable<void> {
    return this.httpClient.post<void>(this.noteApiUrl, requestData);
  }

  updateNote(noteId: string, requestData: NoteFormData): Observable<void> {
    return this.httpClient.put<void>(`${this.noteApiUrl}/${noteId}`, requestData);
  }

  deleteNote(id: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.noteApiUrl}/${id}`);
  }
}
