# Markdown Formatting Rules

**Last Updated:** December 21, 2025

This document defines the formatting standards for Markdown documentation files.

## 1. File Type Detection

Identify file types by examining filename prefix and location:

**Context Document Prefixes** (`.ai-context/` folder):
- `IDEA_*` - Vision & Strategy Documents
- `ROADMAP_*` - Feature Planning Documents  
- `SNIPPET_*` - Code Implementation References (special rules apply)
- `SUMMARY_*` - Quick Reference Guides
- `AUDIT_*` - Assessment Reports

**Toolbox Document Prefixes** (`.ai-toolbox/` folder):
- `SKILL_*` - Executable skills for AI agents
- `RULE_*` - Reusable formatting and validation rules

**Kiro Documents** (`.kiro/` folder):
- Specs: `.kiro/specs/**/*.md`
- Steering: `.kiro/steering/*.md`
- Other Kiro-managed documentation

**Standard Documents**:
- Any other `.md` files in the project

## 2. General Validation Rules

Apply these rules to ALL Markdown files unless specifically exempted.

### 2.1 Last Updated Date

- **Requirement**: MUST have `**Last Updated:** Month DD, YYYY` near top after main title
- **Format**: Use full month name, two-digit day, four-digit year
- **Examples**: 
  - ✅ CORRECT: `**Last Updated:** December 21, 2025`
  - ❌ WRONG: `**Last Updated:** 12/21/2025`
  - ❌ WRONG: `**Last Updated:** Dec 21, 2025`

### 2.2 Heading Numbering

- **H2 Sections**: Number sequentially starting from 1
  - Format: `## 1. Section Name`, `## 2. Next Section`
- **H3 Subsections**: Number relative to parent section
  - Format: `### 1.1 Subsection`, `### 1.2 Another Subsection`
- **H4 Subsubsections**: Do NOT number
  - Format: `#### Unnumbered Heading`
- **H5 and Deeper**: Do NOT use heading format
  - Instead use bold list items: `- **Title:** Content`
- **Maximum Heading Level**: H4 is the deepest allowed
- **Sequential Integrity**: When adding/removing/moving sections, renumber all affected sections

### 2.3 Folder Path Notation

- **Folders/Directories**: MUST include trailing forward slash `/`
  - ✅ CORRECT: `src/Restaurant.API/Controllers/`
  - ❌ WRONG: `src/Restaurant.API/Controllers`
- **Files**: No trailing slash
  - ✅ CORRECT: `src/Restaurant.Domain/OrderService.cs`
  - ❌ WRONG: `src/Restaurant.Domain/OrderService.cs/`

### 2.4 Example Convention

**All examples** in documentation should use a fictional "Restaurant" application.

**Generic paths**:
- `src/Restaurant.API/`
- `src/Restaurant.Domain/`
- `src/Restaurant.Infrastructure/`

**Generic filenames**:
- `OrderController.cs`
- `MenuService.cs`
- `CustomerRepository.cs`

**Code examples**: Demonstrate concepts without project-specific context

### 2.5 Content Formatting

#### Code Snippet Formatting

- Show either complete code OR specific relevant excerpts
- If partial, explain omissions in prose, NOT in code comments
- ❌ WRONG:
  ```csharp
  public class OrderController
  {
      // ... existing code
      public void ProcessOrder() { }
  }
  ```
- ✅ CORRECT: Show complete class OR explain in surrounding text what's omitted

#### Heading Label Conventions

- Use natural language, not code identifiers
- No variable names, method names, or code syntax in headings
- Descriptive and clear without reading content
- ❌ WRONG: `### Adding _customerId Property`
- ❌ WRONG: `### Implementing IOrderService.ProcessOrder()`
- ✅ CORRECT: `### Adding Private Field Property`
- ✅ CORRECT: `### Implementing Order Processing Method`

### 2.6 List Usage

**Prefer lists over comma-separated sentences when presenting multiple items.**

**When to use lists:**
- Multiple paths or filenames
- Multiple requirements or steps
- Multiple examples or options
- Any enumeration of 3+ items

**Examples**:
- ❌ WRONG: "The files include OrderController.cs, MenuService.cs, and CustomerRepository.cs"
- ✅ CORRECT: 
  ```markdown
  The files include:
  - OrderController.cs
  - MenuService.cs
  - CustomerRepository.cs
  ```

**Benefits**: Improves scannability, readability, and clarity

### 2.7 Emoji Usage

Use emojis consistently to provide visual emphasis for specific concepts.

**CRITICAL RULE**: Emojis MUST always be followed by UPPERCASE text for the label/keyword.

#### Boolean Operations

- **Purpose**: Emphasize yes/no or good/bad states
- **Format**: `(✅ YES | ❌ NO)` or `(✅ GOOD | ❌ BAD)` or `(✅ CORRECT | ❌ WRONG)`
- **Usage**: Place before or within the text being emphasized
- **Examples**:
  - ✅ CORRECT: `src/Restaurant.API/Controllers/` (folder with trailing slash)
  - ❌ WRONG: `src/Restaurant.API/Controllers` (folder missing trailing slash)
  - ✅ YES - Feature implemented correctly
  - ❌ NO - Feature requires fixes

#### Priority/Severity Levels

- **Purpose**: Indicate importance, urgency, or severity
- **Format**: `(🚨 CRITICAL | ❌ HIGH | ⚠️ MEDIUM | ℹ️ LOW)`
- **Usage**: Prefix severity level in reports, issues, or prioritized lists
- **Uppercase Requirement**: Always use `🚨 CRITICAL`, never `🚨 Critical` or `🚨 critical`
- **Definitions**:
  - 🚨 **CRITICAL**: Production blockers, security vulnerabilities, data corruption risks
  - ❌ **HIGH**: Major performance issues, architecture violations, maintainability problems
  - ⚠️ **MEDIUM**: Code quality issues, minor performance problems, inconsistencies
  - ℹ️ **LOW**: Style issues, documentation gaps, minor improvements

## 3. File-Specific Rules

### 3.1 SNIPPET_* Files

#### Critical 1:1 Mapping Rule

- MUST maintain section alignment with corresponding ROADMAP_* file
- Section numbers MUST match ROADMAP exactly (e.g., ROADMAP 2.1 = SNIPPET 2.1)
- **Exception**: SNIPPET MAY have gaps in section numbering
  - If ROADMAP has 2.1, 2.2, 2.3, 2.4 but only 2.1, 2.2, 2.4 have code
  - SNIPPET numbering: 2.1, 2.2, 2.4 (gap at 2.3 is intentional)
  - DO NOT renumber 2.4 to 2.3 - preserve original numbers for reference

#### Code Format Requirements

- Use pseudo-code to illustrate concepts, not production-ready code
- Opening curly bracket `{` MUST be on new line (C# standard)
- Use descriptive variable names (avoid abbreviations or single letters)
- ❌ WRONG: `var i`, `var temp`, `var x`
- ✅ CORRECT: `var entityIndex`, `var temporaryValue`, `var xCoordinate`

#### What to EXCLUDE from code

- Copyright notices and file headers
- XML documentation comments
- Using/import statements
- Namespace declarations
- Implementation comments (`//` or `/* */`) if obvious from variable/method names
- File path annotations
- Godot scene files (`*.tscn`)
- Test code and test examples

#### What to INCLUDE in code

- Class/method signatures showing structure
- Essential properties and fields
- Core logic and algorithms
- Descriptive variable names that convey meaning

### 3.2 ROADMAP_* Files

- **Check for Pairing**: If ROADMAP_* exists, corresponding SNIPPET_* should exist
- **Note**: Missing SNIPPET is informational, not critical

### 3.3 Kiro Documents

**Requirements**:
- Apply ALL standard rules for general validation
- Must follow heading numbering convention
- Must update Last Updated date

**Scope**:
- `.kiro/specs/` - Feature specifications
- `.kiro/steering/` - Project steering documents
- Other `.kiro/` managed documentation

### 3.4 SKILL_* Files

**Metadata Section Requirements:**
- MUST have YAML frontmatter at the beginning of file
- Surrounded by `---` dividers (before and after)
- Required attributes: `name`, `description`, `display-name`, `keywords`
- Keywords MUST be lowercase and in array format

**Attribute Specifications:**
- **name**: Unique identifier in kebab-case
  - Example: `validate-order`
- **description**: Comprehensive explanation of skill functionality
- **display-name**: Human-readable title
  - Example: `Validate Order`
- **keywords**: Array of lowercase search terms

**Example** (using Restaurant app):
```yaml
---
name: validate-order
description: Validates restaurant order data including menu items, quantities, customer information, and payment methods
display-name: Validate Order
keywords: [order, validate, menu, customer, payment, restaurant]
---
```

## 4. Priority Levels

### 4.1 CRITICAL (Must fix)

- Missing Last Updated date
- Broken heading numbering sequence
- SNIPPET/ROADMAP section misalignment
- Code identifiers in headings
- H5+ headings (should be bold list items)
- Missing metadata in SKILL files

### 4.2 HIGH (Should fix)

- Incorrect folder path notation (missing or extra `/`)
- Code snippets with ellipsis comments
- Wrong code formatting in SNIPPET files
- Comma-separated items instead of lists (3+ items)

### 4.3 MEDIUM (Nice to fix)

- Inconsistent date format
- Missing SNIPPET file when ROADMAP exists
- Style inconsistencies

## 5. Validation Checklist

Use this systematic checklist to verify all formatting rules have been applied:

### 5.1 General Rules Verification

- [ ] **Last Updated date** present and in correct format (`Month DD, YYYY`)
- [ ] **H2 sections** numbered sequentially starting from 1
- [ ] **H3 sections** numbered relative to parent (e.g., 1.1, 1.2 under section 1)
- [ ] **H4 sections** unnumbered
- [ ] **No H5+ headings** (use bold list items: `- **Title:** Content`)
- [ ] **Folders** have trailing forward slash `/`
- [ ] **Files** have no trailing slash
- [ ] **Examples** use Restaurant application (generic, non-project-specific)
- [ ] **Emojis** followed by UPPERCASE text (e.g., `✅ CORRECT:`, never `✅ Correct:`)
- [ ] **Code snippets** are complete OR explained in prose (no ellipsis comments)
- [ ] **Headings** use natural language (no code identifiers like `_fieldName` or `IService.Method()`)
- [ ] **Multiple items** use lists instead of comma-separated text (3+ items)

### 5.2 File-Specific Verification

For SNIPPET files:
- [ ] **Section numbers** match corresponding ROADMAP file exactly
- [ ] **Code formatting** follows standards (opening brace on new line)
- [ ] **Variable names** are descriptive (not `i`, `temp`, `x`)
- [ ] **Excludes** boilerplate (no copyright, using statements, namespaces, comments)

For ROADMAP files:
- [ ] **Check pairing**: Corresponding SNIPPET file exists (informational)

For Kiro documents:
- [ ] **All general rules** applied
- [ ] **Located** in `.kiro/` folder structure

For SKILL files:
- [ ] **Metadata present** at beginning of file
- [ ] **YAML frontmatter** with `---` dividers
- [ ] **Required attributes** defined (name, description, display-name, keywords)
- [ ] **Keywords lowercase** in array format
- [ ] **Name in kebab-case** format

### 5.3 Automated Verification

Use these regex patterns to detect violations:

**Emoji violations** (should return NO results except explanatory text):
```
(?:✅|❌|🚨|⚠️|ℹ️)\s+[a-z]
```

**Folder path violations** (folders without trailing slash):
```
`[^`]+/[A-Za-z0-9_-]+`(?!\s*\()
```

Note: Manual review still required as regex cannot catch all violations.
