# Shared brief for research agents (batch B1–B6)

Repository: /home/user/echelon-assurance (git branch claude/fervent-tesla-ytnxz6).
Read first: AGENTS.md, SECURITY-AND-CONFIDENTIALITY.md, methodology/research-methodology.md,
methodology/identifiers.md, tools/schema.py (register column definitions).

Rules:
- Write ONLY the files assigned to you. Do NOT git add/commit/push; the coordinator commits.
- Do NOT modify any other repository. Other Echelon repos are read-only, under
  /home/user/{unbound,daf-landscape,Echelon-AI-Native-Enterprise-Design-Framework,echelon-organization-administration,clarity-framework}
  and /tmp/claude-0/-home-user/fdec508f-2ff2-5927-acb3-7afb55343b16/scratchpad/ref/<repo>.
- Never fabricate sources, URLs, dates, prices, or requirement text. If you could not
  read a source, say so and mark access summary-only or the claim as assumption/open-question.
- Tag every significant conclusion with an evidence class: authoritative,
  auditor-interpretation, industry-convention, customer-expectation, vendor-claim,
  recommendation, assumption, open-question. Seek contradicting evidence.
- No secrets, no live security findings, no confidential info (the repo is currently public).
- Today is 2026-11-25; use it as the retrieval date for sources you actually fetched.

Fragment files (CSV, UTF-8, header row exactly as in tools/schema.py, `;` separates list values,
quote fields containing commas):
- sources/fragments/<KEY>.csv — schema "sources". source_id = SRC-<KEY>-NNN.
- regulatory/fragments/<KEY>.csv — schema "requirements" (if assigned). req_id = REQ-<FRAMEWORK>-NNN
  using only the framework codes assigned to you. Leave control_ids empty. open_question_ids may
  reference your OQ ids. source_ids must reference your own SRC ids.
- registers/fragments/<KEY>-open-questions.csv — schema "open_questions", using your assigned OQ range.
  Each question states why internal research cannot resolve it and the lowest-cost credible resolution.
- registers/fragments/<KEY>-control-candidates.md — proposed controls (objective, risk, mechanism,
  target automation level L0–L5 with reasoning, fail-closed vs fail-open, evidence, independence).
  The coordinator builds the unified catalog from these; do not assign CTL ids.

Final message to the coordinator: <= 300 words: files written, top findings, biggest uncertainties.
