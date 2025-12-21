---
name: format-markdown
description: Validates and formats Markdown files against project documentation standards including Last Updated dates, heading numbering, folder paths, emoji usage, and file-specific rules for SNIPPET/ROADMAP pairing
display-name: Format Markdown
keywords: [markdown, format, validate, documentation, heading, emoji, path, snippet, roadmap, kiro, audit]
---

# Format Markdown

**Last Updated:** December 21, 2025

Validates and formats Markdown files according to formatting rules defined in `RULE_Markdown.md`.

## 1. Instructions

When invoked to format or validate a Markdown file:

### 1.1 Load Formatting Rules

Read `RULE_Markdown.md` to understand:
- File type detection (IDEA, ROADMAP, SNIPPET, SUMMARY, AUDIT, SKILL, RULE, Kiro, Standard)
- General validation rules (Last Updated Date, Heading Numbering, Folder Paths, Content Formatting, Emoji Usage)
- File-specific rules (SNIPPET/ROADMAP pairing, code format requirements)
- Priority levels (CRITICAL, HIGH, MEDIUM)
- Validation checklist (Section 5)

### 1.2 Execution Steps

1. **Read the target file** to analyze current state
2. **Detect file type** using filename prefix and location (see RULE_Markdown.md Section 1)
3. **Apply general rules** from RULE_Markdown.md Section 2
4. **Apply file-specific rules** from RULE_Markdown.md Section 3
5. **Generate validation report** with findings organized by priority
6. **Offer to fix issues** if user requests

### 1.3 Self-Verification Protocol

**CRITICAL**: After making ANY changes to a file, perform self-verification BEFORE presenting results:

1. **Use RULE_Markdown.md Section 5** - Go through the complete validation checklist systematically
2. **Re-read the modified file** - Confirm changes were applied correctly
3. **Use regex verification** - Run automated checks from RULE_Markdown.md Section 5.3
4. **Manual spot-checks** - Verify examples, emoji usage, and heading numbering
5. **Only report completion** after confirming 100% compliance

This ensures quality and prevents unnecessary back-and-forth with the user.

### 1.4 Validation Output Format

Present findings in this structure:

```markdown
## Validation Report for [filename]

**File Type**: [SNIPPET/ROADMAP/IDEA/SUMMARY/AUDIT/Kiro/Standard]

### Critical Issues
[List violations from RULE_Markdown.md Section 4.1]

### Style Issues  
[List violations from RULE_Markdown.md Section 4.2-4.3]

### Suggestions
[List recommended improvements]

### File-Specific Notes
[Any special considerations for this file type]
```

For each issue, provide:
- **Line number** (if applicable)
- **Current state** (what exists now)
- **Required state** (what it should be per RULE_Markdown.md)
- **Suggested fix** (exact text to use)

## 2. Usage Examples

**Example 1: Validate a SNIPPET file**
```
Use the "Format Markdown" skill to validate .ai-context/SNIPPET_ScenarioLoader.md
```

**Example 2: Check and fix a Kiro spec**
```
Use the "Format Markdown" skill to check .kiro/specs/feature-x.md and fix any issues
```

**Example 3: Validate heading structure**
```
Use the "Format Markdown" skill to validate heading structure in README.md
```

## 3. Notes

- All formatting rules are defined in `RULE_Markdown.md`
- Always load rules before validation to ensure consistency
- SNIPPET files have special numbering rules (gaps allowed for ROADMAP alignment)
- Maximum heading level is H4; use bold list items for deeper nesting
