import { Injectable, inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

import { UserProfile, UserProfileFormData } from "../models/user-profile.model";
import { environment } from "../../../../environments/environments";


@Injectable({
  providedIn: 'root'
})
export class UserProfileApiService {
  private httpClient = inject(HttpClient);
  private readonly userProfileApiUrl = environment.apiUrl + '/profiles/me';

  getUserProfile(): Observable<UserProfile | null> {
    return this.httpClient.get<UserProfile | null>(this.userProfileApiUrl);
  }

  updateUserProfile(requestData: UserProfileFormData): Observable<void> {
    return this.httpClient.put<void>(this.userProfileApiUrl, requestData);
  }
}