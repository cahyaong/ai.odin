---
name: audit-codebase
description: Conducts codebase audits with configurable personas, generates AUDIT_*.md reports with severity ratings and recommendations
display-name: Audit Codebase
keywords: [audit, codebase, quality, assessment, security, performance, architecture]
example-prompt:
  - Audit the Restaurant codebase for code quality issues
  - Can you perform a security audit on src/Restaurant.API/?
  - Run a performance analysis of the OrderService
---

# SKILL: Audit Codebase

**Last Updated:** January 3, 2026

---

## Table of Contents

- [SKILL: Audit Codebase](#skill-audit-codebase)
  - [Table of Contents](#table-of-contents)
  - [1. Overview](#1-overview)
  - [2. Audit Types](#2-audit-types)
  - [3. Execution Workflow](#3-execution-workflow)
  - [4. Report Structure](#4-report-structure)
  - [5. Self-Assessment Protocol](#5-self-assessment-protocol)
  - [6. Historical Tracking](#6-historical-tracking)

---

Conducts codebase audits using expert personas from `RULE_Persona.md`. Generates `AUDIT_*.md` reports in `.ai-context/`. For severity levels, see `RULE_Markdown.md` Section 6.

## 1. Overview

**Capabilities:**
- Full `Source/` directory scanning
- Persona-driven analysis (see `RULE_Persona.md`)
- Dual-depth reporting (high-level + detailed)
- Severity classification (🚨 CRITICAL, ❌ HIGH, ⚠️ MEDIUM, ℹ️ LOW)
- Actionable recommendations with file paths and code examples
- Historical tracking via Appendix

## 2. Audit Types

| Category             | Audit Types                                                                                                         |
|----------------------|---------------------------------------------------------------------------------------------------------------------|
| **General Software** | Code Quality, System Architecture, Performance, Security, Asset Management, DevOps Infrastructure, User Experience |
| **Game Development** | Game Design, Level Design, Narrative Design, Audio Design, QA Testing                                               |
| **Documentation**    | Technical Writing, Information Architecture                                                                         |
| **Research**         | ML Research, Data Science                                                                                           |

See `RULE_Persona.md` Section 2 for persona definitions and readiness criteria.

## 3. Execution Workflow

**Setup:**
1. If audit type not specified, ask user to select
2. Confirm persona perspective before proceeding

**Analysis:**
1. **Scan** - Recursively analyze `Source/` directory
2. **High-level** - Architectural patterns, system-wide trends, dependencies
3. **Deep-dive** - Specific issues with file paths, line numbers, code snippets
4. **Classify** - Apply severity ratings per `RULE_Markdown.md` Section 6

**Generate Report:**
1. Check for existing `AUDIT_[Type].md`, move to Appendix if exists
2. Create new report using structure (Section 4)
3. Run self-assessment protocol (Section 5)
4. Validate with `SKILL_FormatMarkdown`
5. Save to `.ai-context/AUDIT_[Type].md`

## 4. Report Structure

| Section                      | Contents                                                            |
|------------------------------|---------------------------------------------------------------------|
| **Header**                   | Title (`# [Type] Audit Report`), Project, Date, Reviewer, Scope     |
| **Executive Summary**        | 2-3 sentence assessment, Grade (A-F), Strengths (✅), Issues (🚨❌)  |
| **High-Level Analysis**      | Architectural Patterns, System-Wide Trends                          |
| **Detailed Analysis**        | Numbered categories; each issue: path, severity, code, fix          |
| **Priority Recommendations** | Immediate (Sprint), Short-term (2 Weeks), Medium-term (1 Month)     |
| **Production Readiness**     | Status, Required Work estimate, Risk Areas                          |
| **Appendix**                 | Historical Audits (if applicable)                                   |

## 5. Self-Assessment Protocol

**🚨 CRITICAL:** Before presenting results, verify using this checklist:

**Completeness:**
- [ ] Executive summary includes overall grade and key issues
- [ ] High-level analysis covers architectural patterns and trends
- [ ] Detailed analysis includes specific file paths and line numbers
- [ ] All severity ratings assigned consistently
- [ ] Priority recommendations organized by timeframe
- [ ] Production readiness assessment provided
- [ ] Historical audits moved to Appendix if applicable

**Severity Rating Accuracy:**
- [ ] 🚨 CRITICAL: Only production blockers, security vulnerabilities, data corruption
- [ ] ❌ HIGH: Major performance issues, architecture violations
- [ ] ⚠️ MEDIUM: Code quality issues, minor performance problems
- [ ] ℹ️ LOW: Style issues, documentation gaps
- [ ] Each severity has clear justification

**Actionable Recommendations:**
- [ ] Each recommendation has specific action description
- [ ] File paths are accurate and complete
- [ ] Code examples show both problem and solution
- [ ] Timeframe estimates are realistic

**Final Validation:**
- [ ] Run `SKILL_FormatMarkdown` on generated file
- [ ] Confirm 100% compliance with `RULE_Markdown.md`
- [ ] Tables follow `RULE_Markdown.md` Section 4.3 padding rules
- [ ] Re-read generated file to confirm accuracy
- [ ] Only report completion after verification passes

## 6. Historical Tracking

**Archive Format:**
- Section title: `## Appendix: Historical Audits`
- Subsection: `### Audit from [Month DD, YYYY]`
- Order: Latest to oldest
- Preserve complete content

**Guidelines:**
- Track resolved issues and grade progression
- Note newly introduced problems
- Compare metrics over time

**Note:** AUDIT reports use actual project examples (Restaurant app requirement applies only to RULE_*/SKILL_* files).
