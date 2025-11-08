# Product Requirements Document (PRD) – VibeTravels

## 1. Product Overview
VibeTravels is an MVP web app that converts simple trip notes into actionable itineraries using AI. The core of the MVP is full CRUD for trip notes, plus AI-powered plan generation that respects a user’s basic travel preferences and parameters such as number of days and travelers. The app operates only in English and uses a minimal email + password account system.

In scope for MVP:
- Email + password authentication
- User profile with basic travel preferences
- Full CRUD for notes with a minimal required structure
- AI plan generation using a selected note and basic parameters (days, travelers)
- Plan history with accept/reject (optional rejection reason) and editable accepted plans
- Simple light/dark theming
- Telemetry for profile completion, plan counts, and accept/reject decisions

## 2. User Problem
Planning engaging trips is hard. People keep fragmented, unstructured ideas about destinations and wishlists but lack a bridge from rough notes to a coherent plan that matches their style, time window, and group size. Users need a lightweight way to capture ideas, then quickly turn them into day-structured plans (when trip length is known) or activity lists (when it is not), with the flexibility to iterate, accept, or discard outcomes.

## 3. Functional Requirements

### 3.1 Authentication and Access
- Sign up/sign in/sign out via email + password (no OAuth, no password reset in MVP)
- Authenticated access required for all user resources

### 3.2 Profile and Preferences
- Profile page with basic attributes: travel style, accommodation type, climate/region
- Preferences optional but recommended; the app gently prompts users to complete them before plan generation

### 3.3 Notes (Core CRUD)
- Create/read/update/delete notes
- Required field: title/location and free-text body with text from 1k - 10k characters
- Users can select exactly one note as input to a plan

### 3.4 Trip Parameters and AI Generation
- Dedicated fields for number of days and number of travelers (separate from note body)
- If days are provided, output is a day-by-day itinerary; otherwise, an activity list
- No AI usage limits in MVP
- Multiple generations can run in parallel; each generation has its own status

### 3.5 Plans Management
- Plan history with metadata (source note, parameters, timestamps)
- Accept or reject plans; optional rejection reason
- Accepted plan is “ready to use” but remains editable

### 3.6 Telemetry
- Track profile completion
- Track plan generations per user per year
- Track accept/reject decisions and optional rejection reasons

### 3.7 Non-functional/UX
- English-only UI (no i18n)
- Simple light/dark theme toggle

## 4. Product Boundaries

In scope:
- Email + password auth (no password reset)
- Notes CRUD with required title/location
- Exactly one note as input to a plan
- AI generation with daily vs list output based on provided days
- Parallel generations
- Plan acceptance/rejection with optional reason and editability post-acceptance
- Basic telemetry and simple theming

Out of scope (MVP):
- Sharing plans across accounts
- Rich media handling (images/video analysis)
- Advanced time/logistics (booking, budgets, maps, transport optimization)
- Internationalization and multi-language
- Advanced roles/permissions
- Folders/archives for notes
- Billing/quotas/usage limits

Assumptions:
- All user content is private to the owner
- Plan content is stored in a structured JSON document rendered in the UI

## 5. User Stories

US-01
- Title: Authenticate with email and password
- Description: As a user, I want to create an account and sign in using my email and password so that I can access my notes, profile, and plans.
- Acceptance criteria:
  - Creating an account with a unique email establishes a session.
  - Signing in with valid credentials establishes a session; invalid credentials yield a generic error.
  - Signing out clears the session and protects private routes.

US-02
- Title: Manage basic travel preferences
- Description: As a signed-in user, I want to set my travel style, accommodation type, and climate/region so AI plans can better match my needs.
- Acceptance criteria:
  - Saving preferences marks the profile as complete.
  - Preferences can be empty; if empty at plan generation, I see a non-blocking prompt with a link to the profile.
  - Updates persist and are reflected immediately.

US-03
- Title: Manage trip notes (CRUD)
- Description: As a signed-in user, I want to create, view, edit, and delete notes to capture ideas for future trips.
- Acceptance criteria:
  - Creating a note requires title/location and description body with required characters from 1k to 10k.
  - Notes list shows only my notes with creation/update timestamps.
  - Editing updates the note and timestamp; deletion requires confirmation.
  - Access to other users’ notes is blocked (403).

US-04
- Title: Prepare a trip request (select one note and parameters)
- Description: As a signed-in user, I want to select one note and specify days and travelers to define the input for AI plan generation.
- Acceptance criteria:
  - I can select exactly one note for a single request.
  - Days and travelers accept positive integers or can be left blank.
  - Invalid values are rejected with clear messages.

US-05
- Title: Generate a plan with AI
- Description: As a signed-in user, I want the system to create a plan based on my selected note, preferences, and parameters.
- Acceptance criteria:
  - If days are provided, the plan is structured by Day 1..N; otherwise, it is a list of activities.
  - Each day/activity includes succinct descriptions.
  - Multiple generations can run concurrently; each shows queued/running/succeeded/failed status.
  - On failure, I see a clear error and a Retry action that creates a new job.

US-06
- Title: Browse plans and details
- Description: As a signed-in user, I want to review generated plans and open any plan to see its full content.
- Acceptance criteria:
  - List of plans with timestamps, structure type (daily/list), and source note/parameters.
  - Opening a plan displays its full content and metadata.

US-07
- Title: Accept, reject, and edit plans
- Description: As a signed-in user, I want to accept a plan as ready to use, reject a plan with an optional reason, and edit accepted plans if needed.
- Acceptance criteria:
  - Accepting sets the plan’s status to Accepted while remaining editable.
  - Rejecting records status Rejected; an optional free-text reason can be saved.
  - Edited accepted plans retain Accepted status and update the timestamp.

US-08
- Title: Secure access to user-owned data
- Description: As the system, I must ensure each user can only access and modify their own notes, trip requests, and plans.
- Acceptance criteria:
  - All protected endpoints verify ownership; cross-user access attempts return 403.
  - Direct URL access to others’ resources is denied.

US-09
- Title: Record key usage telemetry
- Description: As product stakeholders, we want to capture essential events to measure profile completion, plan generation volume, and decision outcomes.
- Acceptance criteria:
  - On first successful save of a complete profile, a profile-completed event is recorded.
  - Each plan generation records a plan-created event with user ID and timestamp.
  - Each accept/reject records a decision event; rejection may include an optional reason.

US-10
- Title: English-only UI and simple theming
- Description: As a user, I want the interface to be clear and consistent in English with a basic light/dark theme toggle.
- Acceptance criteria:
  - All UI text is in English; there are no language switches.
  - Theme toggle applies immediately and persists per user/session.

## 6. Success Metrics

### 6.1 Profile completion rate
- Target: 90% of active users have a completed profile.
- Measurement: profile-completed events over active users, reported on rolling 30-day and annual views.

### 6.2 Plan generation engagement
- Target: 75% of users generate 3 or more plans per calendar year.
- Measurement: count of plan-created events per user per year; percentage of users meeting or exceeding 3.