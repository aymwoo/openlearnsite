# Phase 5: Teacher activity publish foundation - Discussion log

> **Audit trail only.** Do not use as input to planning, research, or
> execution agents.
> Decisions are captured in `05-CONTEXT.md`.

**Date:** 2026-04-11
**Phase:** 05-teacher-activity-publish-foundation
**Areas discussed:** publish record lifecycle, publish visibility control,
student activity foundation, confirm action payload

---

## Publish record lifecycle

| Option | Description | Selected |
|--------|-------------|----------|
| Update one linked AI activity for the course | Re-publishing resolves to the same linked activity record for that course. | ✓ |
| Create a new activity every time | Each publish creates another activity entry. | |
| Ask each time | Teacher chooses update vs create during re-publish. | |

**User's choice:** Update one linked AI activity for the course.
**Notes:** The linked AI-backed activity should behave like one durable course
activity record, not a growing list of near-duplicates.

---

## Publish visibility control

| Option | Description | Selected |
|--------|-------------|----------|
| Unpublished by default | Teacher must explicitly enable student visibility in the confirm flow. | ✓ |
| Published by default | Publish remains explicit, but the default state is on. | |

**User's choice:** Unpublished by default.
**Notes:** This intentionally differs from the legacy manual mission-add page,
which currently defaults `CheckPublish` to checked.

---

## Student activity foundation

| Option | Description | Selected |
|--------|-------------|----------|
| `showmission.aspx` / `Ltype=1` / `Mupload=true` | Reuse the upload-capable mission route as the phase foundation. | ✓ |
| `description.aspx` / `Ltype=6` / `Mupload=false` | Use a simpler read-only description route first. | |

**User's choice:** `showmission.aspx` / `Ltype=1` / `Mupload=true`.
**Notes:** This keeps Phase 5 aligned with later submission and completion work
instead of starting from the read-only description branch.

---

## Confirm action payload

| Option | Description | Selected |
|--------|-------------|----------|
| Same full generated content goes to both | Course content and student activity receive the same body. | |
| Compact course block + full student content | Course gets a short publish block while student activity gets the full content. | |
| Selected append-only course sections + full student content | Course keeps teacher-selected appended sections while student activity stores the full generated instructions. | ✓ |

**User's choice:** Selected append-only course sections for the course, full
generated instructions for the student activity.
**Notes:** This preserves the Phase 4 lesson-content pattern while keeping the
student activity executable and complete.

---

## The agent's discretion

- Pick the smallest reliable linkage model for finding the existing AI-backed
  mission/menu pair on re-publish.
- Fit the confirm UI into `teacher/courseedit.aspx` without introducing a new
  standalone teacher surface.
- Choose the exact publish endpoint contract as long as it stays authenticated,
  course-authorized, and keeps lesson/menu visibility synchronized.

## Deferred ideas

None.
