---
name: review-markdown
description: Optimizes Markdown documentation for conciseness and clarity while reducing token usage for AI agent consumption
display-name: Review Markdown
keywords: [review, optimize, concise, token, markdown, documentation, refactor, streamline]
example-prompt:
  - Review and optimize RULE_RestaurantCoding.md for token reduction
  - Can you make SKILL_ValidateOrder.md more concise?
  - Reduce redundancy in IDEA_ReservationSystem.md
---

# SKILL: Review Markdown

**Last Updated:** January 3, 2026

---

## Table of Contents

- [SKILL: Review Markdown](#skill-review-markdown)
  - [Table of Contents](#table-of-contents)
  - [1. Overview](#1-overview)
    - [1.1 Persona Usage](#11-persona-usage)
    - [1.2 Location-Based Optimization](#12-location-based-optimization)
  - [2. Analysis Phase](#2-analysis-phase)
  - [3. Optimization Techniques](#3-optimization-techniques)
    - [3.1 Table Conversion](#31-table-conversion)
    - [3.2 Cross-Reference Replacement](#32-cross-reference-replacement)
    - [3.3 Section Consolidation](#33-section-consolidation)
    - [3.4 Example Reduction](#34-example-reduction)
  - [4. Execution Workflow](#4-execution-workflow)
  - [5. Self-Verification Protocol](#5-self-verification-protocol)
  - [6. Output Report](#6-output-report)

---

Optimizes Markdown documentation for conciseness and clarity while preserving essential content. Reduces token consumption for AI agent processing. Uses `SKILL_FormatMarkdown` for validation.

## 1. Overview

**Main Goals (in priority order):**

1. **Completeness** - Add sufficient detail to avoid ambiguity
2. **Clarity** - Make content concise and clear to avoid confusion
3. **Token Optimization** - Reduce document length to minimize AI agent token consumption

**Priority Rationale:** Completeness prevents misinterpretation. Clarity ensures understanding. Token optimization is secondary—never sacrifice accuracy for brevity.

**When to Use:**
- Documentation exceeds reasonable length for its purpose
- Redundancy detected across sections
- Content duplicated from referenced files
- Tables would better represent structured data

### 1.1 Persona Usage

**Default Persona:** Technical Writer (see `RULE_Persona.md` Section 2.3)

All Markdown reviews adopt the Technical Writer perspective, focusing on:
- Clarity and accuracy of content
- Style consistency across documents
- Audience-appropriate language
- Logical organization and flow

**Document-Specific Personas:**

| Document Type | Additional Personas                           |
|---------------|-----------------------------------------------|
| `RULE_*`      | Information Architect (taxonomy, navigation)  |
| `SKILL_*`     | Information Architect + Software Engineer     |
| `IDEA_*`      | Game Designer or relevant domain expert       |
| `SNIPPET_*`   | Software Engineer (code accuracy)             |
| `AUDIT_*`     | Relevant audit persona from `RULE_Persona.md` |
| `SUMMARY_*`   | Information Architect (discoverability)       |

**Multi-Persona Workflow:**
1. Start with Technical Writer lens for structure and clarity
2. Apply document-specific persona for domain accuracy
3. Synthesize recommendations from both perspectives
4. Note any conflicts between personas with trade-off discussion

### 1.2 Location-Based Optimization

**🚨 CRITICAL:** Determine optimization tier FIRST before applying any techniques.

**Tier Classification:**

| Location             | Audience         | Tier         | Target Reduction |
|----------------------|------------------|--------------|------------------|
| `.ai-toolbox/`       | Agents           | Aggressive   | 50-70%           |
| `.ai-context/`       | Humans + Agents  | Balanced     | 30-50%           |
| `AGENTS.md` (root)   | Agents           | Aggressive   | 50-70%           |
| Other root `*.md`    | Humans           | Conservative | 20-40%           |

**Tier Comparison:**

| Aspect                  | Aggressive              | Balanced                       | Conservative                |
|-------------------------|-------------------------|--------------------------------|-----------------------------|
| **Language**            | Terse, minimal          | Concise, clear                 | Cohesive, flowing           |
| **Tables**              | Prefer over all prose   | Use for structured data        | Use when aids readability   |
| **Examples**            | Single pair max         | Reduce but keep essential      | Keep if aids understanding  |
| **Abbreviations**       | Acceptable              | Common ones acceptable         | Spell out first use         |
| **Cross-references**    | Replace aggressively    | Replace with brief context     | Preserve context            |
| **Sentence Fragments**  | Acceptable              | Acceptable in lists/tables     | Avoid                       |
| **Explanation Depth**   | Minimal                 | Sufficient for context         | Full explanation            |

**Tier Selection Workflow:**
1. Identify file location
2. Match to tier classification table
3. Apply tier-appropriate techniques throughout review
4. Verify reduction target aligns with tier

## 2. Analysis Phase

**Initial Assessment:**
1. Read entire document
2. Record metrics (lines, H2 sections, TOC entries)
3. **Determine optimization tier** per Section 1.2
4. **Run SKILL_FormatMarkdown** to identify existing issues

**Redundancy Types:**

| Type                            | Indicator                          | Solution                        |
|---------------------------------|------------------------------------|---------------------------------|
| **Duplication across sections** | Same concept in 2+ places          | Consolidate into single section |
| **Verbose descriptions**        | Lists that could be tables         | Convert to table format         |
| **Repeated examples**           | Multiple examples of same concept  | Single correct/wrong pair       |
| **External file duplication**   | Content in referenced files        | Replace with cross-reference    |
| **Scattered related content**   | Related topics non-adjacent        | Merge into logical groups       |

**Essential Content (Must Preserve):**
- All critical rules (🚨 CRITICAL, ❌ HIGH severity)
- Self-verification checklists
- Cross-references to other files
- Unique information not available elsewhere

## 3. Optimization Techniques

### 3.1 Table Conversion

Before (verbose):
```markdown
- **OrderService**: Handles order processing including validation...
- **MenuService**: Manages menu items including availability...
```

After (table):
```markdown
| Service          | Responsibilities                                     |
|------------------|------------------------------------------------------|
| **OrderService** | Order validation, pricing, kitchen notifications     |
| **MenuService**  | Availability, pricing updates, seasonal items        |
```

**Tier Guidance:**
- **Aggressive:** Convert ALL structured lists to tables
- **Balanced:** Convert lists with 3+ items or parallel structure
- **Conservative:** Convert only when tables improve readability

### 3.2 Cross-Reference Replacement

Before: `## Severity Levels [Full definitions repeated]`  
After: `For severity levels, see RULE_Markdown.md Section 6.`

**Tier Guidance:**
- **Aggressive:** Replace all duplicated content with references
- **Balanced:** Replace with brief one-sentence context + reference
- **Conservative:** Keep summary if context aids understanding, add reference

### 3.3 Section Consolidation

Before: `## 2. Setup`, `## 3. Config`, `## 4. Test`, `## 15. Quick Ref`  
After: `## 2. Quick Start` with subsections 2.1-2.3

**Tier Guidance:**
- **Aggressive:** Merge all related sections aggressively
- **Balanced:** Merge sections with significant overlap
- **Conservative:** Merge only sections with near-identical content

### 3.4 Example Reduction

Before (multiple pairs): 3+ wrong/correct pairs for same concept  
After (single pair): One `// ❌ WRONG` + One `// ✅ CORRECT`

**Tier Guidance:**
- **Aggressive:** Single example pair per concept
- **Balanced:** 1-2 example pairs, keep edge cases if critical
- **Conservative:** Keep examples that aid comprehension

## 4. Execution Workflow

**4.1 Pre-Optimization:**
1. Read entire document
2. Record before-metrics (lines, sections, TOC)
3. Identify document type (RULE, SKILL, IDEA, etc.)
4. **Determine optimization tier** per Section 1.2
5. **Run SKILL_FormatMarkdown** for baseline compliance
6. List essential content to preserve

**4.2 Optimization:**
1. Present analysis to user with identified redundancies
2. **Specify tier and target reduction range**
3. Get user approval before changes
4. Apply tier-appropriate optimizations:
   - Convert verbose lists to tables
   - Replace duplicated content with cross-references
   - Consolidate scattered sections
   - Reduce examples per tier guidance
5. Update Last Updated date if content changes
6. Preserve all checklists (add if missing)
7. **Run SKILL_FormatMarkdown** to validate changes

**4.3 Post-Optimization:**
1. **Run SKILL_FormatMarkdown** final validation
2. Verify all critical content preserved
3. Calculate reduction metrics
4. **Verify reduction within tier target range**
5. Present results to user

**4.4 SNIPPET Implementation Audit (SNIPPET Files Only):**

**Trigger:** When reviewing SNIPPET files in `.ai-context/`

**Process:**
1. Identify corresponding IDEA file (e.g., `SNIPPET_OrderProcessing.md` → `IDEA_OrderProcessing.md`)
2. Analyze codebase for implemented features:
   - Check existing source files for matching components/services
   - Review recent git commits for implementation evidence
3. For each SNIPPET section:
   - Compare code snippet against actual implementation
   - If fully implemented: Mark section for removal
   - If partially implemented: Keep snippet for unimplemented portions
4. If ALL sections implemented:
   - Recommend deleting entire SNIPPET file
   - Reference `RULE_Document.md` Section 3.4 for lifecycle rules

**Example:** `SNIPPET_MenuService.md` shows `GetAvailableItems()` implementation. If `src/Restaurant.Domain/MenuService.cs` contains matching implementation, remove that section.

**Verification:**
- [ ] Codebase files checked against SNIPPET code
- [ ] Git history reviewed for implementation commits
- [ ] Corresponding IDEA sections cross-referenced
- [ ] Removal recommendations documented with evidence

## 5. Self-Verification Protocol

**🚨 CRITICAL:** Before presenting results, verify using this checklist:

**Tier Verification:**
- [ ] Optimization tier correctly identified per Section 1.2
- [ ] Tier-appropriate techniques applied throughout
- [ ] Reduction percentage within tier target range

**Essential Content:**
- [ ] All critical rules (🚨/❌ severity) preserved
- [ ] Self-verification checklists maintained or added
- [ ] Cross-references accurate and files exist
- [ ] Key workflows and processes intact

**Structural Quality (Tier-Aware):**

| Check                                     | Aggressive    | Balanced       | Conservative   |
|-------------------------------------------|---------------|----------------|----------------|
| Tables used for structured data           | Required      | Preferred      | When helpful   |
| Cross-references replace duplication      | All cases     | Most cases     | With context   |
| Related sections consolidated             | Aggressively  | Where overlap  | Near-identical |
| Examples reduced                          | Single pair   | 1-2 pairs      | As needed      |

**Formatting Compliance:**
- [ ] **SKILL_FormatMarkdown** validation passed
- [ ] Last Updated date current (if content changed)
- [ ] Heading numbering follows RULE_Markdown.md
- [ ] Tables follow `RULE_Markdown.md` Section 4.3 padding rules
- [ ] TOC updated to reflect new structure

**Metrics Verified:**
- [ ] Before and after line counts recorded
- [ ] Reduction percentage calculated
- [ ] Reduction within tier target (Aggressive: 50-70%, Balanced: 30-50%, Conservative: 20-40%)
- [ ] Section and TOC counts compared

## 6. Output Report

**Present results in this format:**

**Optimization Tier:** [Aggressive/Balanced/Conservative] (Target: [X-Y%])

| Metric      | Before | After | Reduction |
|-------------|--------|-------|-----------|
| Lines       | [X]    | [Y]   | [Z%]      |
| Sections    | [X]    | [Y]   | [Z%]      |
| TOC Entries | [X]    | [Y]   | [Z%]      |

**Key Changes:**
- [Consolidated sections]
- [Converted to tables]
- [Replaced with cross-references]

**Verification:**
- ✅ All critical rules preserved
- ✅ Self-verification checklists maintained
- ✅ SKILL_FormatMarkdown validation passed
- ✅ Reduction within tier target range
