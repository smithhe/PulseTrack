## Planify Feature Overview and Fullstack Instructions (Node.js Frontend, .NET API)

### Feature list
- **Tasks**
  - Create/edit/delete; content and Markdown description; labels; priority; pin/unpin; complete/uncomplete; subtasks; change history
  - Due dates, time, timezone, recurrence (daily/weekly/monthly/yearly/custom with count/until), natural language parsing
  - Reminders (picker, suggestions); notifications
- **Projects and sections**
  - Create/edit/delete projects; color/icon; sections within projects
  - Views: list and board (kanban-like); reordering sections
- **Filters and special views**
  - Inbox, Today, Scheduled (day/month/range), Tomorrow, Anytime, Repeating, Unlabeled, All Items, Completed, Labels view, Priority views (1–4), Pinboard
- **Calendar and scheduling**
  - Date/time pickers; repeat configuration; calendar UI (week/month); calendar events generation; relative date display; overdue/today/upcoming states
- **Labels**
  - Create/edit/delete; assign/unassign; label picker; labels summary; labels page
- **Search and quick interactions**
  - Quick Find across filters, projects, sections, items, labels
  - Quick Add dialog/app; natural language dates; OS search provider integration
- **Backups and migration**
  - Create/import/restore backups; import overview
  - Migration from Planner database
- **Preferences and system integration**
  - Preferences pages: Backup, Sidebar; What's New
  - Notifications service; time monitor; desktop search provider integration

## Fullstack blueprint (Electron-ready Node.js frontend + .NET API backend)

### Assumptions
- **Frontend**: Node.js + React + TypeScript, built with Vite; designed to be wrapped in Electron later (use `electron-vite` or custom Electron builder). No authentication.
- **Backend**: ASP.NET Core 8 Web API (C#) + EF Core + PostgreSQL.
- **Background jobs**: Hangfire (in-process or separate worker).
- **Search**: PostgreSQL full-text (optionally replaceable by Meilisearch/Elasticsearch later).
- **Scope**: Single-tenant (no users/auth). All data is app-local. No external service integrations or account syncing.

### Architecture
- Two apps in a monorepo: `frontend/` (Node.js app) and `api/` (.NET API).
- API is system of record; frontend communicates via REST/JSON and optionally SSE/WebSockets for live updates.
- Background workers handle reminders, calendar event materialization, and search indexing.

- Backend follows Clean Architecture and is split into separate class library projects with strict dependencies:
  - `Api` (ASP.NET Core Web API host using FastEndpoints) → depends on `Application`
  - `Application` (use cases, DTOs, interfaces/ports) → depends on `Domain`
  - `Domain` (entities, value objects, enums, domain services, repository interfaces) → depends on nothing
  - `Infrastructure` (adapters implementing ports: EF Core, storage, notifications, external providers) → depends on `Application` and `Domain`
  - `Infrastructure.Persistence` (DbContext, configurations, migrations) → depends on `Infrastructure`
  - `BackgroundJobs.Worker` (Hangfire server/worker) → depends on `Application` and `Infrastructure`
  - `Shared` (cross-cutting abstractions/utilities if needed) → referenced where appropriate

- Dependency rules: `Domain` is pure and isolated; `Application` references only `Domain`; `Api` references `Application` (and only uses `Infrastructure` via DI registration in composition root); `Infrastructure*` projects implement interfaces from `Application`/`Domain` and are wired up at startup.

- Suggested folder layout under `Api/`:
  - `Api/src/Api`
  - `Api/src/Application`
  - `Api/src/Domain`
  - `Api/src/Infrastructure`
  - `Api/src/Infrastructure.Persistence`
  - `Api/src/BackgroundJobs.Worker`
  - `Api/tests/*` (unit/integration test projects per layer)

### Data model (single-tenant: omit user tables and user_id columns)
- **Project**(id, name, color, icon, sort_order, is_inbox, created_at, updated_at)
- **Section**(id, project_id, name, sort_order, created_at, updated_at)
- **Item**(id, project_id, section_id, content, description_md, pinned, priority, completed, completed_at, created_at, updated_at)
- **SubItem**(id, parent_item_id, content, completed, sort_order)
- **Label**(id, name, color, created_at, updated_at)
- **ItemLabel**(item_id, label_id)
- **DueDate**(item_id, date_utc, timezone, is_recurring, recurrence_type, recurrence_interval, recurrence_count, recurrence_end_utc, recurrence_weeks)
- **Reminder**(id, item_id, remind_at_utc, timezone, meta_json)
- **ItemHistory**(id, item_id, change_type, before_json, after_json, changed_at)
- **Backup**(id, file_url, created_at)
- **SearchIndex**(entity_type, entity_id, content_tsvector)

### API surface (REST, JSON; versioned at `/api/v1`)
- **Projects/Sections**
  - GET/POST `/projects`; GET/PUT/DELETE `/projects/{id}`
  - GET/POST `/projects/{id}/sections`; PUT/DELETE `/sections/{id}`
  - POST `/projects/{id}/reorder-sections`
- **Items**
  - GET `/items?projectId=&labelId=&filter=&q=&dueFrom=&dueTo=`
  - POST `/items`; GET/PUT/DELETE `/items/{id}`
  - POST `/items/{id}/complete`; POST `/items/{id}/uncomplete`
  - POST `/items/{id}/pin`; POST `/items/{id}/unpin`
  - POST `/items/{id}/move` (project/section/order)
  - GET `/items/{id}/history`
- **Subitems**
  - POST `/items/{id}/subitems`; PUT/DELETE `/subitems/{id}`
- **Labels**
  - GET/POST `/labels`; PUT/DELETE `/labels/{id}`
  - POST `/items/{id}/labels` (assign/unassign)
- **Due dates and reminders**
  - PUT `/items/{id}/due-date`
  - GET/POST `/items/{id}/reminders`; PUT/DELETE `/reminders/{id}`
  - POST `/parse/nlp-datetime` → structured due/reminder from natural language
- **Views/Filters**
  - GET `/views/today`, `/views/scheduled`, `/views/pinboard`, `/views/completed`, `/views/priority/{1..4}`, `/views/tomorrow`, `/views/anytime`, `/views/repeating`, `/views/unlabeled`, `/views/all`
- **Calendar**
  - GET `/calendar/events?from=&to=` returns events derived from items (due/reminders/recurrence)
- **Search and quick find**
  - GET `/search?q=` unified results: filters, projects, sections, items, labels
  - POST `/quick-add` `{ text }` → `{ item draft + parsed due/reminders }`
- **Backups/Migration**
  - POST `/backups/create`; GET `/backups`
  - POST `/backups/import` (upload); POST `/backups/{id}/restore`
  - POST `/migrations/planner` (file upload + dry-run/apply)
- **System**
  - GET `/health`, `/metrics`

### Backend services (ASP.NET Core)
- **Layers**: API → Application (use cases) → Domain → Infrastructure (EF Core, providers, storage)
- **Background jobs**
  - Reminder dispatch (scan upcoming reminders; enqueue notification jobs)
  - Calendar event materialization/cache
  - Search indexing updates on write
- **NLP date parsing**
  - Implement with Microsoft Recognizers-Text or similar; expose `/parse` endpoint

### Frontend app (Node.js, Electron-ready)
- **Stack**: React + TypeScript with Vite; structure for Electron wrapping (shared preload, IPC channels as needed later). No authentication flows.
- **State/data**: React Query for data fetching; optimistic updates for quick add, complete/uncomplete, pin/unpin; state in URL where applicable.
- **Core pages**
  - Inbox, Today, Scheduled (day/month/range), Labels, Pinboard, Completed, Priority 1–4, Tomorrow, Anytime, Repeating, Unlabeled, All
  - Project pages (list and board views); section reorder; drag-and-drop
- **Components**
  - Task row/card with due state, recurrence badge, reminders, labels, pin, priority, markdown description viewer/editor
  - DateTimePicker, RepeatConfig, ReminderPicker, LabelPicker, Project/Section pickers
  - Calendar (week/month); QuickFind modal; QuickAdd bar/modal
- **Integrations**
  - Desktop notifications; later switch to Electron notifications
- **Accessibility/i18n**
  - ARIA-friendly UI; `react-intl` or `next-intl` alternative for Vite; localized strings

### Search
- PostgreSQL full-text indices on content/description; trigram for fuzzy matching
- Endpoint returns typed results with highlights; frontend groups results by type (filters/projects/items/labels)

### Backups/Migration
- Export JSON manifest; import with dry-run preview and mapping
- Restore with confirmation; record import/restore actions in an audit/history table

### Security and data handling (no auth in scope)
- Rate limiting
- Validation and sanitization of Markdown

### Observability/ops
- Structured logging; OpenTelemetry traces; health checks
- CI/CD with build, unit/integration tests, migrations, smoke tests
- EF Core migrations; feature flags for new providers where needed

### Acceptance criteria (samples)
- Scheduled view: shows items grouped by date; overdue/today/upcoming styling
- Board view: drag item across sections persists immediately
- Backups: export and re-import reproduces projects, sections, items, labels, reminders


