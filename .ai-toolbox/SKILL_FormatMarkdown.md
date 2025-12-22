---
name: format-markdown
description: Formats and validates Markdown files against project documentation standards including Last Updated dates, heading numbering, folder paths, emoji usage, and file-specific rules for SNIPPET/ROADMAP pairing
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

#### Standard Processing (< 1,000 lines)

For files under 1,000 lines, process the entire file at once:

1. **Read and analyze** the target file
2. **Detect file type** (see RULE_Markdown.md Section 1)
3. **Validate in priority order**:
   - CRITICAL issues first, starting with heading numbering (RULE_Markdown.md Section 4.1)
   - HIGH and MEDIUM issues next (RULE_Markdown.md Sections 4.2-4.3)
4. **Generate validation report** with findings organized by priority
5. **Apply fixes** if user requests (update Last Updated date ONLY if content changes - see RULE_Markdown.md Section 2.1)

#### Large File Batch Processing (>= 1,000 lines)

For files with 1,000+ lines, use adaptive batch processing to minimize context window pressure:

**Initial Assessment:**

1. **Count total lines** and identify all H2 section boundaries
2. **Calculate initial batch size**: 3-5 H2 sections per batch
   - Adjust based on section complexity (length, code blocks, nested structure)
   - Simpler sections: Use larger batches (4-5 sections)
   - Complex sections: Use smaller batches (3 sections)
3. **Create batch plan**: Map which H2 sections belong to each batch

**Batch Processing Loop:**

For each batch, execute the following workflow:

1. **Report Progress**: "Processing batch X of Y (Sections M.0 through N.0)"
2. **Load Batch Context**: Read the H2 sections in current batch
3. **Detect file type**: Identify SNIPPET, ROADMAP, SKILL, etc.
4. **Apply all validation rules**:
   - General rules from RULE_Markdown.md Section 2
   - File-specific rules from RULE_Markdown.md Section 3
5. **Apply fixes** to issues found in current batch
6. **Self-verify batch changes**: Use validation checklist from RULE_Markdown.md Section 5
7. **Handle failures**: If batch validation fails:
   - Reduce batch size to 1-2 sections
   - Retry with smaller batch
   - Report adjusted strategy: "Batch too complex, processing 1 section at a time"

**Post-Processing:**

1. **Final full-file verification**: After all batches complete
   - Verify H2 numbering sequence across entire file
   - Check cross-batch consistency
2. **Update Last Updated date**: ONLY if any content changes were made (see RULE_Markdown.md Section 2.1)
3. **Generate consolidated report**: Combine findings from all batches
4. **Report completion**: Summary of all batches processed

**Batch Processing Guidelines:**

- **Adaptive Sizing**: Adjust batch size based on:
  - Section line count (longer sections = smaller batches)
  - Code block density (more code = smaller batches)
  - Table complexity (complex tables = smaller batches)
  - Nested list depth (deeper nesting = smaller batches)

- **Progress Reporting**: After each batch, report:
  - Batch number and total (e.g., "Batch 2 of 5")
  - Section range processed (e.g., "Sections 3.0-5.0")
  - Issues found and fixed count
  - Next batch preview

- **Error Recovery**: On batch failure:
  - Split failing batch into smaller chunks (1-2 sections)
  - Process problematic sections individually
  - Document strategy adjustment
  - Continue with remaining batches

- **Cross-Batch Consistency**:
  - Track H2 numbering across all batches
  - Maintain running issue list
  - Last Updated date modified only in final pass
  - Final verification ensures file-wide coherence

### 1.3 Self-Verification Protocol

**CRITICAL**: After making ANY changes to a file, perform self-verification BEFORE presenting results:

1. **Use RULE_Markdown.md Section 5** - Go through the complete validation checklist systematically
2. **Re-read the modified file** - Confirm changes were applied correctly
3. **Verify Last Updated date**: If content changed, date updated; if no changes, date unchanged (see RULE_Markdown.md Section 2.1)
4. **Use regex verification** - Run automated checks from RULE_Markdown.md Section 5.3
5. **Manual spot-checks** - Verify examples, emoji usage, and heading numbering
6. **Only report completion** after confirming 100% compliance

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

**Example 4: Process large file with batch mode**
```
Use the "Format Markdown" skill to validate SUMMARY_LargeCodebase.md
```
(File with 1,500 lines automatically processed in adaptive batches with progress reporting)

## 3. Notes

- All formatting rules defined in `RULE_Markdown.md` (single source of truth)
- **Heading numbering errors are highest priority** - check these first
- **Last Updated date modified only when content changes** (see RULE_Markdown.md Section 2.1)
- Files with 1,000+ lines use adaptive batch processing automatically
- SNIPPET files have special numbering rules (gaps allowed for ROADMAP alignment)
