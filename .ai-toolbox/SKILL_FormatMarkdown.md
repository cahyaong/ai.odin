---
name: format-markdown
description: Validates and formats Markdown files against RULE_Markdown.md and RULE_Document.md standards
display-name: Format Markdown
keywords: [markdown, format, validate, documentation, heading]
example-prompt:
  - Format IDEA_MenuService.md
  - Can you validate the Markdown in SNIPPET_OrderProcessing.md?
  - Check if RULE_RestaurantCoding.md follows our documentation standards
---

# SKILL: Format Markdown

**Last Updated:** January 3, 2026

---

## Table of Contents

- [SKILL: Format Markdown](#skill-format-markdown)
  - [Table of Contents](#table-of-contents)
  - [1. Execution Steps](#1-execution-steps)
  - [2. Large File Processing](#2-large-file-processing)
  - [3. Table Validation](#3-table-validation)
    - [3.1 Detection Algorithm](#31-detection-algorithm)
    - [3.2 Alignment Verification](#32-alignment-verification)
  - [4. Self-Assessment Protocol](#4-self-assessment-protocol)
  - [5. Output Response Template](#5-output-response-template)

---

Validates Markdown files against `RULE_Markdown.md` (formatting) and `RULE_Document.md` (content).

## 1. Execution Steps

1. **Load rules** from `RULE_Markdown.md` and `RULE_Document.md`
2. **Read and analyze** target file
3. **Detect file type** by prefix (SNIPPET, IDEA, SKILL, etc.)
4. **Validate in priority order:**
   - 🚨 CRITICAL: Heading numbering, TOC, Last Updated date
   - ❌ HIGH: Path notation, code snippets, table column alignment
   - ⚠️ MEDIUM: Emoji usage, date format
5. **Check file-specific rules:**
   - SNIPPET: Section alignment with IDEA
   - IDEA: No language/framework specifics
   - SKILL: YAML metadata present
   - RULE/SKILL: Restaurant app examples
6. **Apply fixes** if requested (update Last Updated only if content changes)
7. **Self-verify** before presenting results

## 2. Large File Processing

For files ≥1,000 lines, use batch processing:

**Setup:**
- Count lines, identify H2 boundaries
- Plan 3-5 H2 sections per batch

**Per Batch:**
1. Report progress: "Batch X of Y (Sections M-N)"
2. Load and validate batch sections
3. Apply fixes
4. On failure: Reduce to 1-2 sections, retry

**Completion:**
- Final full-file verification
- Update Last Updated if content changed
- Generate consolidated report

## 3. Table Validation

### 3.1 Detection Algorithm

**Table Identification:**
1. Scan for lines starting and ending with `|`
2. Group consecutive matching lines as table blocks
3. Parse each table:
   - Row 1: Header row
   - Row 2: Separator row (contains `-` and `|`)
   - Rows 3+: Data rows

**Column Parsing:**
1. Split each row by `|` delimiter
2. Trim leading/trailing empty elements
3. Count columns (must be consistent across all rows)

### 3.2 Alignment Verification

**For each table, verify:**

| Check                    | Rule                                            | Severity |
|--------------------------|-------------------------------------------------|----------|
| Column count consistency | All rows must have same column count            | HIGH     |
| Cell padding             | Each cell padded to match column max width      | HIGH     |
| Separator dash length    | Dashes must match column width                  | HIGH     |
| Minimum padding          | At least one space on each side of cell content | HIGH     |

**Verification Steps:**
1. Calculate `max_width[col]` from longest cell in each column
2. Check each cell: `content_width + 2` must equal `max_width[col]`
3. Check separator: dash count must equal `max_width[col] - 2`

**Report Format:** `Line N: Table column X - width mismatch (found Y, expected Z)`

## 4. Self-Assessment Protocol

**🚨 CRITICAL:** Before presenting results, verify using this checklist:

**File Verification:**
- [ ] Re-read modified file
- [ ] Verify changes applied correctly
- [ ] Last Updated: Updated only if content changed

**Structural Compliance:**
- [ ] TOC: Present, all H2/H3 with anchor links
- [ ] Tables: All columns have consistent padding widths
- [ ] Restaurant examples: Used in RULE_*/SKILL_* files

**Final Validation:**
- [ ] Use validation checklists from both RULE files

## 5. Output Response Template

```markdown
## Validation Report for [filename]

**File Type**: [SNIPPET/IDEA/SKILL/etc.]

### Critical Issues
[List from RULE_Markdown.md Section 6.1]

### Table Issues
[List table alignment violations with line numbers and column details]

### Style Issues
[List from RULE_Markdown.md Sections 6.2-6.3]

### File-Specific Notes
[Special considerations for this file type]
```

For each issue:
- Line number
- Current state → Required state
- Suggested fix
