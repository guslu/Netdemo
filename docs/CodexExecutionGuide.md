# Working Effectively with Codex on This Repository

This guide explains why prior runs may have looked incomplete and how to prompt Codex so work lands as real, reviewable implementation.

## What likely happened in earlier runs

When a prompt is very broad ("build a production-ready SaaS system"), Codex often does the safest first milestone: scaffolding, architecture, and planning.
That behavior is especially likely when the prompt itself says to scaffold first and avoid implementing everything at once.

In this repository, that first milestone is mostly complete (solution split, baseline wiring, API/frontend scaffolds, CI/Docker placeholders).

## Was Codex lacking access?

Usually, partial delivery in this project is **not** an access issue. It is commonly one of these:

1. **Scope too large for one iteration**
   - A full enterprise SaaS includes dozens of endpoints, policies, migrations, UI states, and tests.
2. **Milestone ambiguity**
   - "Production-ready" + "start with scaffold" can cause conservative interpretation.
3. **Missing acceptance criteria per increment**
   - Without explicit DoD, Codex may stop once structure compiles and baseline tests pass.
4. **Environment/tooling mismatch**
   - In some sessions, EF migration tooling or runtime dependencies may be unavailable.

## Prompting pattern that works better

Use **small, verifiable increments** and require concrete outputs.

### Recommended prompt template

```text
Continue from current branch.

Goal for this increment:
- Implement <specific feature slice>.

Definition of done:
- Add/modify these files: <paths or areas>
- API behavior: <exact endpoints/status/error behavior>
- Authorization rules: <who can do what>
- Persistence: <entities/migrations/seeding requirements>
- Tests: <exact unit/integration cases>
- Docs: update DeliveryStatus and README with what changed.

Validation commands to run:
- dotnet test
- (any other required commands)

Constraints:
- Keep Clean Architecture boundaries.
- No placeholders/TODO-only output.
- If blocked by environment, state exact blocker and provide the patch up to blocker.
```

## Example next increment for this repo

A strong next request would be:

1. Implement `POST /api/v1/auth/register` with role assignment guard (Admin only for elevated roles).
2. Add refresh token support and revoke endpoint.
3. Add EF migration for identity + domain schema.
4. Add integration tests for login/register/refresh happy and unauthorized paths.

This creates a fully testable vertical slice, instead of expanding breadth only.

## How to tell Codex to avoid "scaffold-only" outcomes

Include these lines explicitly:

- "Do not stop at structure creation; implement working endpoints and tests in this same run."
- "If you stop early, explain exactly which acceptance criteria are unmet."
- "Return a completion checklist marking each requirement done/pending with file references."

## Practical checklist before each run

- Confirm current branch and base state (`git status`).
- State one milestone only.
- Require tests for that milestone.
- Require updated status doc after implementation.
- Keep each milestone to work that can be completed and verified in one run.

## Suggested milestone sequence

1. Auth vertical slice (register/login/refresh/revoke + tests).
2. Tenant enforcement (organization-scoped queries + policies + tests).
3. Projects/Tasks CRUD with role constraints + tests.
4. Comments + audit logging behaviors + tests.
5. Deployment hardening and CI quality gates.

This sequence turns the existing scaffold into production-capable functionality without architecture churn.
