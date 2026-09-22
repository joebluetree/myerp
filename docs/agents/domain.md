# Domain Docs

How the engineering skills should consume this repo's domain documentation when exploring the codebase.

This is a **multi-context** repo.

## Before exploring, read these

- **`CONTEXT-MAP.md`** at the repo root — it points at one `CONTEXT.md` per context. Read each one relevant to the topic.
- **`backend/MyErp.<Module>/CONTEXT.md`** — the glossary for that bounded context.
- **`docs/adr/`** — system-wide decisions. Read ADRs that touch the area you're about to work in.
- **`backend/MyErp.<Module>/docs/adr/`** — decisions scoped to a single context.

If any of these files don't exist, **proceed silently**. Don't flag their absence; don't suggest creating them upfront. The `/domain-modeling` skill (reached via `/grill-with-docs` and `/improve-codebase-architecture`) creates them lazily when terms or decisions actually get resolved.

## Contexts

The bounded contexts are the four business modules. This is not a naming
convention — architecture rule 4 in `README.md` enforces it: the modules never
reference each other, so a term may legitimately mean different things in each.

| Context   | Glossary                              | Context ADRs                           |
| --------- | ------------------------------------- | -------------------------------------- |
| Masters   | `backend/MyErp.Masters/CONTEXT.md`    | `backend/MyErp.Masters/docs/adr/`      |
| Admin     | `backend/MyErp.Admin/CONTEXT.md`      | `backend/MyErp.Admin/docs/adr/`        |
| Accounts  | `backend/MyErp.Accounts/CONTEXT.md`   | `backend/MyErp.Accounts/docs/adr/`     |
| Inventory | `backend/MyErp.Inventory/CONTEXT.md`  | `backend/MyErp.Inventory/docs/adr/`    |

`MyErp.Common`, `MyErp.Data` and `MyErp.Entities` are a **shared kernel**, not a
context. They hold no context-specific vocabulary. A term that needs to live
there is by definition one every context agrees on — treat promoting a term into
the shared kernel as an ADR-worthy decision, not a refactor.

The Angular frontend has no `CONTEXT.md` of its own. `frontend/src/app/features/<module>/`
mirrors the backend contexts and inherits their vocabulary; a screen in
`features/inventory/` uses the Inventory glossary.

## File structure

```
/
├── CONTEXT-MAP.md                          ← points at each context
├── docs/adr/                               ← system-wide decisions
├── backend/
│   ├── MyErp.Common/                       ← shared kernel, no CONTEXT.md
│   ├── MyErp.Data/                         ← shared kernel, no CONTEXT.md
│   ├── MyErp.Entities/                     ← shared kernel, no CONTEXT.md
│   ├── MyErp.Masters/
│   │   ├── CONTEXT.md
│   │   └── docs/adr/                       ← context-specific decisions
│   ├── MyErp.Admin/
│   ├── MyErp.Accounts/
│   └── MyErp.Inventory/
└── frontend/                               ← mirrors backend contexts
```

Note this repo uses `backend/MyErp.<Module>/` where the skill templates assume
`src/<context>/`. The paths above are the real ones.

## Use the glossary's vocabulary

When your output names a domain concept (in an issue title, a refactor proposal, a hypothesis, a test name), use the term as defined in the relevant context's `CONTEXT.md`. Don't drift to synonyms the glossary explicitly avoids.

In a multi-context repo, **always say which context you mean** when a term is ambiguous across them. If the concept you need isn't in that context's glossary yet, that's a signal — either you're inventing language the project doesn't use (reconsider) or there's a real gap (note it for `/domain-modeling`).

## Flag ADR conflicts

If your output contradicts an existing ADR, surface it explicitly rather than silently overriding:

> _Contradicts ADR-0007 (event-sourced orders) — but worth reopening because…_

A context ADR binds only its own module. A system-wide ADR in `docs/adr/` binds
every context; contradicting one is a bigger deal, so say so.
