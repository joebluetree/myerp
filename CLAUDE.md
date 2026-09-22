# MyErp

See `README.md` for layout, prerequisites, how to run the stack, and the seven
architecture rules the skeleton exists to protect. Those rules are binding —
breaking one is a design decision, not a shortcut.

## Agent skills

### Issue tracker

Issues and specs are local markdown under `.scratch/<feature-slug>/`, not GitHub Issues. See `docs/agents/issue-tracker.md`.

### Triage labels

The five canonical roles, unchanged, recorded as a `Status:` line in each issue file. See `docs/agents/triage-labels.md`.

### Domain docs

Multi-context: a root `CONTEXT-MAP.md` over one `CONTEXT.md` per business module. See `docs/agents/domain.md`.
