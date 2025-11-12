# UI Architecture for VibeTravels

## 0. Current Route Map (MVP Active)

| Route           | Purpose                                                | Notes                                                                  |
| --------------- | ------------------------------------------------------ | ---------------------------------------------------------------------- |
| `/home`         | Landing/Home                                           | Lightweight introduction; not core to flow                             |
| `/notes`        | Notes list + inline note detail/edit + plan generation | Cards expose actions: view (expand/modal), edit, delete, generate plan |
| `/plans`        | List generated plans                                   | Top annotation instructs generation from `/notes`                      |
| `/auth/sign-in` | Sign in                                                | Email + password                                                       |
| `/auth/sign-up` | Registration                                           | Email + password                                                       |
| `/user/profile` | Preferences                                            | Simple travel preferences                                              |

## 0.1 Plan Generation Flow (Simplified)

1. User clicks generate icon/button on a note card in `/notes`.
2. Inline form (modal or expandable panel) collects minimal data (parameters deferred; currently may require none).
3. Submit attempts generation:

- Success: plan persisted, status `ready`; redirect to `/plans`.
- AI-only failure (partial): plan persisted with status `failed`; redirect to `/plans` (failed badge shown).
- Total failure: snackbar error; remain on `/notes`.

4. If a plan already exists for that note, generation action is disabled (one plan per note MVP constraint).

## 1. UI Structure Overview

A responsive single-page application (SPA) with a persistent app shell:

- **Top navbar** with: brand, primary nav (**Notes**, **Plans**), and a **Profile menu** (Profile page, Theme toggle, Sign out).
- **Authenticated routes only** for user resources; **/auth** is the only public route.
- **Detail views** for Notes and Plans open via deep-linkable routes; **Generate Plan** is a drawer/dialog launched from **Note Detail**.
- **English-only UI** with light/dark theme.
- **Status-first Plans UX** using tabs and filter chips driven by API enums; polling on plan detail while generation is in progress.
- **Accessibility** (WCAG 2.2 AA) and **security** (route guards, JWT in memory via interceptor) designed-in.

## 2. View List

### A) Auth

- **Path**: `/auth`
- **Main purpose**: Sign up/in via email + password; establish an authenticated session.
- **Key information to display**:
  - Email, password fields; toggle for **Sign in** / **Create account**.
  - Generic error for invalid credentials.
- **Key view components**:
  - `AuthForm` (mode switcher: sign-in / sign-up)
  - `AuthCard` (responsive container)
  - `SessionRedirector` (route to last-intended URL or `/notes`)
- **UX, accessibility, and security considerations**:
  - Keyboard-first flow; show/hide password with ARIA labeling.
  - Avoid password strength meters (not required in MVP).
  - Show single generic error to avoid leaking validity states.
  - On success, store JWT **in memory**; clear on sign-out.
- **Mapped PRD stories**: US-01.

### B) Notes List

- **Path**: `/notes` (query params: `q`, `sort`, `order`, `cursor`)
- **Main purpose**: Browse, search, and open user notes; create new note.
- **Key information to display**:
  - User’s notes only with title, location, updated/created timestamps.
  - Soft-deleted notes hidden by default.
- **Key view components**:
  - `NotesToolbar` (search input, sort dropdown, “New note” button)
  - `NotesList` (cards/table hybrid; infinite scroll with cursor)
  - `EmptyState` (no notes yet → CTA to create)
- **UX, accessibility, and security considerations**:
  - Large clickable row with clear focus outlines.
  - Announce pagination loads to screen readers.
  - 403 protection via server; client hides cross-user content.
- **Mapped PRD stories**: US-03, US-08, US-10.

### C) Note Detail (Create/Edit)

- **Path**: `/notes` displayed in dialog
- **Main purpose**: Create or edit a note; launch **Generate Plan**.
- **Key information to display**:
  - Required fields: **title** (1–200), **location** (1–255), **bodyMd** (1000–10000).
  - Timestamps; ownership (implicitly the current user).
- **Key view components**:
  - `NoteEditor` with:
    - Live character counters/validators
    - Markdown editor + Preview (`EditorTabs`)
    - Autosave to localStorage until body ≥ 100 chars, then enable Save
  - `NoteActions` (Save, Delete with confirm)
  - `GeneratePlanButton` (opens drawer/dialog)
- **UX, accessibility, and security considerations**:
  - Explain validation early; disable Save until minimum body length.
  - Confirm destructive actions;
  - Keep JWT in memory; PUT is full replace—guard unsaved changes with leave prompts.
- **Mapped PRD stories**: US-03, US-04, US-08, US-10.

### D) Generate Plan Drawer/Dialog (from Note Detail)

- **Path**: In-place drawer/dialog;
- **Main purpose**: Select parameters (days, travelers, start date) and queue a plan.
- **Key information to display**:
  - Non-blocking **Profile completion** nudge with link to Profile.
  - Input validation: positive integers or blank.
- **Key view components**:
  - `GeneratePlanForm` (days, travelers, start date)
  - `ProfileNudgeBanner` (status fetched from `/profiles/me`)
  - `SubmitAction` (POST `/plans`, then navigate to new Plan Detail)
- **UX, accessibility, and security considerations**:
  - Form described-by helper text; focus return to origin on close.
  - Clear messaging for invalid values.
- **Mapped PRD stories**: US-02, US-04, US-05.

### E) Plans List

- **Path**: `/plans` (query params for filters: `decisionStatus`, `generationStatus`, `noteId`, `structureType`, `sort`, `order`, `cursor`)
- **Main purpose**: Browse plans across states; open a plan detail; filter by status/structure/note.
- **Key information to display**:
  - Status chips for `generationStatus` and `decisionStatus`.
  - Source note, params (days/travelers/start date), timestamps.
- **Key view components**:
  - `PlansTabs`: **All**, **In progress**, **Ready**, **Accepted**, \*\*Rejected`
  - `FilterChips` (noteId, structureType) + `EnumChips` (from `/meta/enums`)
  - `PlansList` (cards with `StatusChip`)
  - `InfiniteScroll`
- **UX, accessibility, and security considerations**:
  - Use ARIA `tablist` semantics; maintain focus on tab switch.
  - Avoid color-only status; include icons and text.
- **Mapped PRD stories**: US-06, US-07, US-10.

### F) Plan Detail

- **Path**: `/plans` displayed in dialog
- **Main purpose**: Show full plan, metadata, and lifecycle actions; poll while generating.
- **Key information to display**:
  - Metadata: source note link, params, structure type, daysCount, timestamps, statuses, error message (if any).
  - Content renderer: **Daily** (Day 1..N) or **Activity List**.
  - “Edited” badge if `adjustedByUser=true`.
- **Key view components**:
  - `PlanHeader` (metadata, `StatusChip`s, badges)
  - `PlanRenderer` (Daily/List switch based on `structureType`)
  - `PlanActions`:
    - Accept / Reject (optional reason)
    - Retry / Cancel (if running; best-effort)
    - Edit (inline editor for content JSON/Markdown)
  - `GenerationPoller` (exponential backoff until terminal state)
  - `DecisionDialog` (reject reason optional)
- **UX, accessibility, and security considerations**:
  - Announce status updates to assistive tech (ARIA live region).
  - Confirm Accept (explain uniqueness rule if 409 conflicts with existing accepted plan for note).
  - Keep Accepted state after edits; show updated timestamp.
- **Mapped PRD stories**: US-05, US-06, US-07, US-08, US-09, US-10.

### G) Profile

- **Path**: `/profile`
- **Main purpose**: Manage preferences (travel style, accommodation type, climate/region).
- **Key information to display**:
  - Profile completeness indicator
- **Key view components**:
  - `ProfileForm` (preferences optional; save marks profile complete)
  - `ThemeToggle` (optimistic update persisted via `/profiles/me`)
  - `CompletionStatus` (nudge in Generate drawer when incomplete)
- **UX, accessibility, and security considerations**:
  - Use descriptive labels with examples.
  - Persist immediately; show success/failure toasts with retry.
- **Mapped PRD stories**: US-02, US-10, US-09.

### H) Not Found / Error

- **Path**: `*` (catch-all), error boundaries
- **Main purpose**: Friendly 404 and structured error panels for API errors.
- **Key information to display**:
  - Error envelope fields `{ code, message, details?, correlationId }`.
- **Key view components**:
  - `NotFound`
  - `ErrorPanel` with expandable technical details
- **UX, accessibility, and security considerations**:
  - Clear recovery CTAs (Back to Notes/Plans).
  - Preserve privacy; avoid echoing PII in errors.
- **Mapped PRD stories**: US-08, US-10 (general UX).

## 3. User Journey Map

### Primary Journey: From idea → accepted plan

1. **Authenticate**: `/auth/sign-in` → redirect to `/notes`.
2. **Capture idea**: `/notes` → Add Note (inline or modal). Save creates note card.
3. **Generate plan**: Click generate on note card → inline form → submit.
4. **Outcome handling**:

- Success or partial failure → redirect `/plans` (list shows new plan with status badge).
- Total failure → snackbar on `/notes` (no plan added).

5. **Browse plans**: `/plans` list only; annotation reminds generation occurs in Notes.
6. **Profile (optional)**: `/user/profile` preferences improve generation quality later (AI integration stage). Non-blocking.

### Secondary Journeys

- Profile-first remains (active).
- Cancel running generation deferred (no running state UI yet).

## 4. Layout and Navigation Structure

- **Global App Shell**

  - **Navbar**: Brand (click → `/notes`), Primary Nav (**Notes**, **Plans**), Profile menu (Profile, Theme toggle, Sign out).
  - **Route Guard**: Protects all routes except `/auth`; redirects 401 to `/auth`.
  - **Theme**: Light/dark controlled at root.

- **Routing**

  - Public: `/auth/sign-in`, `/auth/sign-up`, `home`
  - Private: `/home`, `/notes`, `/plans`, `/user/profile`, `*` → NotFound

- **Navigation Patterns**
  - **Deep links** for Note/Plan detail.
  - **Modal/drawer routes** for Generate Plan (`/notes/:noteId?generate=open`) enable back/forward compatibility.
  - **Tabs** on Plans List reflect filter state via query parameters for shareability.

## 5. Key Components

1. **AppHeader**: Navbar with primary nav, theme toggle, profile menu.
2. **RouteGuard**: Evaluates authentication; reroutes unauthenticated users to `/auth`.
3. **HttpInterceptor**: Injects Bearer token; handles error envelope and 401 redirects.
4. **NotesList / PlansList**: Card/table hybrids with infinite scroll using `cursor`.
5. **StatusChip**: Reusable status indicator for `generationStatus` and `decisionStatus` (text + icon; accessible contrast).
6. **NoteEditor**: Title/location inputs, Markdown body with live counters and Preview tab; autosave drafts.
7. **GeneratePlanForm**: Days/travelers/start date with validation; profile completion banner.
8. **PlanRenderer**: Renders canonical plan JSON as Daily or Activity List; supports inline edit.
9. **PlanActions**: Accept/Reject (optional reason), Retry, Cancel; guarded by generation state.
10. **GenerationPoller**: Exponential backoff polling of `GET /plans/:id` until terminal state.
11. **ProfileForm**: Preferences editor.
12. **FilterChips & EnumChips**: Drive consistent filtering across views using `/meta/enums`.
13. **ConfirmDialog**: Destructive action confirmations with clear consequences.
14. **ErrorPanel & Snackbar**: Standardized display of `{ code, message, details?, correlationId }`.
15. **EmptyState**: Contextual CTAs when no results (notes/plans).

---

### API Compatibility Summary (per view)

- **Auth**: `POST /users/signin`, `POST /users/signup`
- **Profile**: `GET /profiles/me`, `PUT /profiles/me`
- **Notes**: `GET /notes`, `POST /notes`, `GET /notes/{id}`, `PUT /notes/{id}`, `DELETE /notes/{id}`
- **Plans (Planned)**: `POST /plans`, `GET /plans` (minimal); others deferred
- **Enums/Meta**: `GET /meta/enums`

### Edge Cases & Error States (handled in views/components)

- Invalid credentials → generic error on `/auth`.
- 401/expired session → redirect to `/auth`; preserve intended URL.
- 403 cross-user access → blocked by server; show “Not permitted” message with link home.
- Note validation failures (length limits) → inline errors + disabled Save.
- Plan creation with invalid params (days/travelers/start date) → form-level errors.
- Generation fails → if partial, appears in list with failed badge; no retry (deferred).
- Cancel (deferred), accept/reject (deferred), conflict handling (deferred).
- Soft-deleted notes invisible by default;
- Network/offline → retry toasts; preserve drafts locally.
- Editing an accepted plan preserves **Accepted** state; show “Edited” badge.

### Requirements → UI Elements Mapping (examples)

- **US-02** Profile completion: `ProfileForm`, `CompletionStatus`, `ProfileNudgeBanner`.
- **US-03** Notes CRUD: `NotesList`, `NoteEditor`, `ConfirmDialog`.
- **US-04** Single-note request with params: `GeneratePlanForm` attached to a specific note.
- **US-05** AI generation & statuses: `GenerationPoller`, `StatusChip`, `Retry/Cancel` in `PlanActions`.
- **US-06** Browse plans & open detail: `PlansList` with tabs, `PlanDetail`.
- **US-07** Accept/Reject/Edit: `PlanActions`, `DecisionDialog`, `PlanRenderer` with inline editor, “Edited” badge.
- **US-08** Secure access: `RouteGuard`, server-side 403; shallow error surfaces.
- **US-09** Telemetry hooks (backend): Triggered on profile completion, plan creation, accept/reject; surfaced via `HttpInterceptor`/services.
- **US-10** English-only + theme: Copy-only English; `ThemeToggle`.
