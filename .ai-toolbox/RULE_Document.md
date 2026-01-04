# RULE: Document Content

**Last Updated:** January 3, 2026

---

## Table of Contents

- [RULE: Document Content](#rule-document-content)
  - [Table of Contents](#table-of-contents)
  - [1. File Type Detection](#1-file-type-detection)
    - [1.1 Global Content Restrictions](#11-global-content-restrictions)
  - [2. Example Conventions](#2-example-conventions)
  - [3. SNIPPET Files](#3-snippet-files)
    - [3.1 Section Mapping Rule](#31-section-mapping-rule)
    - [3.2 Code Format](#32-code-format)
    - [3.3 Content Restrictions](#33-content-restrictions)
    - [3.4 SNIPPET Lifecycle](#34-snippet-lifecycle)
  - [4. IDEA Files](#4-idea-files)
    - [4.1 Purpose](#41-purpose)
    - [4.2 Allowed vs Prohibited](#42-allowed-vs-prohibited)
    - [4.3 Structure](#43-structure)
  - [5. SKILL Files](#5-skill-files)
    - [5.1 Metadata Requirements](#51-metadata-requirements)
  - [6. Validation Checklist](#6-validation-checklist)
    - [6.1 SNIPPET Files](#61-snippet-files)
    - [6.2 IDEA Files](#62-idea-files)
    - [6.3 SKILL Files](#63-skill-files)
    - [6.4 RULE/SKILL Examples](#64-ruleskill-examples)

---

Content rules for document types. For Markdown formatting, see `RULE_Markdown.md`. For severity levels, see `RULE_Markdown.md` Section 6.

## 1. File Type Detection

| Prefix      | Location       | Purpose                                   |
|-------------|----------------|-------------------------------------------|
| `IDEA_*`    | `.ai-context/` | Game feature discussions and ideas        |
| `ROADMAP_*` | `.ai-context/` | Prioritized development features (future) |
| `SNIPPET_*` | `.ai-context/` | Code implementation references            |
| `SUMMARY_*` | `.ai-context/` | Quick reference guides                    |
| `AUDIT_*`   | `.ai-context/` | Assessment reports                        |
| `SKILL_*`   | `.ai-toolbox/` | Executable AI agent skills                |
| `RULE_*`    | `.ai-toolbox/` | Formatting and validation rules           |
| `*.md`      | `.kiro/`       | Kiro specs and steering docs              |

### 1.1 Global Content Restrictions

**Applies to:** All file types EXCEPT `ROADMAP_*`

| Prohibited Content                               | Rationale                           |
|--------------------------------------------------|-------------------------------------|
| Priority markers (P0, P1, Phase 1, Priority 1)   | Belongs in ROADMAP files only       |
| Effort estimation (story points, hours)          | Belongs in project management tools |
| Implementation timelines or roadmaps             | Belongs in ROADMAP files only       |
| Implementation status (Not Started, In Progress) | Belongs in project management tools |
| Status emojis (for progress)                     | Belongs in project management tools |

**Exception:** `ROADMAP_*` files MAY contain priorities, timelines, and status tracking.

**Rationale:** Planning, prioritization, and status tracking belong in dedicated ROADMAP files or project management tools. All other documentation focuses on technical content only.

## 2. Example Conventions

**🚨 CRITICAL:** RULE_* and SKILL_* files MUST use Restaurant application examples.

**Applies to:** `.ai-toolbox/` files only  
**Does NOT apply to:** Project-specific docs (IDEA, SNIPPET, AUDIT, etc.)

| Category | Examples                                                        |
|----------|-----------------------------------------------------------------|
| Paths    | `src/Restaurant.API/`, `src/Restaurant.Domain/`                 |
| Files    | `OrderController.cs`, `MenuService.cs`, `CustomerRepository.cs` |
| Concepts | Order, Menu, Customer, Table, Reservation, Payment, Kitchen     |

## 3. SNIPPET Files

### 3.1 Section Mapping Rule

**🚨 CRITICAL:** SNIPPET sections MUST align 1:1 with corresponding IDEA file.

- Section numbers must match exactly (IDEA 2.1 = SNIPPET 2.1)
- Gaps allowed if IDEA section has no code (2.1, 2.2, 2.4 is valid)
- Do NOT renumber to close gaps

### 3.2 Code Format

For complete C# code rules, see `RULE_CSharp.md` Section 3.

**Quick Reference:**

| Include                     | Exclude                                 |
|-----------------------------|-----------------------------------------|
| Class/method signatures     | Copyright, using statements, namespaces |
| Essential properties/fields | XML documentation comments              |
| Core logic/algorithms       | Obvious constructors (param→field only) |
| Descriptive variable names  | Implementation comments if obvious      |

### 3.3 Content Restrictions

**🚨 CRITICAL:** SNIPPET files contain ONLY code examples. No project management content.

See **Section 1.1 Global Content Restrictions** for prohibited content.

**Allowed Content:**

| Allowed                           | Notes                      |
|-----------------------------------|----------------------------|
| Code snippets with context        | Primary content type       |
| Cross-references to IDEA sections | For traceability           |
| Integration notes (technical)     | How systems interact       |
| Performance characteristics       | Complexity, memory, timing |
| Data flow descriptions            | Input/output relationships |

### 3.4 SNIPPET Lifecycle

**Deletion Criteria:**
- SNIPPET file may be **deleted entirely** when ALL features from corresponding IDEA file are implemented
- Individual sections may be **removed** when their corresponding IDEA sections are implemented
- Implementation verification requires: codebase analysis AND recent git commit review

**Valid States:**

| State           | Description                                     |
|-----------------|-------------------------------------------------|
| Full SNIPPET    | IDEA has no implemented features                |
| Partial SNIPPET | Some IDEA sections implemented, others pending  |
| No SNIPPET      | All IDEA features fully implemented in codebase |

**Note:** Missing SNIPPET file is valid if corresponding IDEA features are fully implemented.

## 4. IDEA Files

### 4.1 Purpose

Feature discussion documents describing WHAT and WHY, not HOW (implementation).

**Audience:** Engineers from any language background with CS fundamentals.

### 4.2 Allowed vs Prohibited

See **Section 1.1 Global Content Restrictions** for prohibited project management content.

| Allowed                                     | Prohibited (Language-Specific)                |
|---------------------------------------------|-----------------------------------------------|
| A*, Dijkstra, genetic algorithms            | C# keywords (class, interface, var)           |
| Array, hash table, quadtree, priority queue | .NET libraries (LINQ, YamlDotNet)             |
| O(n), O(log n), complexity analysis         | ECS terms (Component, System, Entity as code) |
| Processor, module, subsystem (architecture) | Factory pattern, Repository pattern (as code) |
| Caching, memoization, lazy evaluation       | async/await, Task, IEnumerable                |

**Rationale:** IDEA files contain raw, uncommitted ideas from discussions. No implementation commitments exist until ideas are pulled to ROADMAP files.

### 4.3 Structure

1. **Purpose & Goals** - Problem statement, objectives, success criteria
2. **Functional Requirements** - Core capabilities, user scenarios
3. **Non-Functional Requirements** - Performance targets, quality attributes
4. **Architecture & Design** - Algorithms, data structures, module structure
5. **Dependencies & Constraints** - Prerequisites, limitations, risks

**Example:** ❌ "Implement RejuvenationSystem inheriting BaseFixedSystem" → ✅ "Rejuvenation processor updates vitality using cached decay rates with O(1) lookup"

## 5. SKILL Files

### 5.1 Metadata Requirements

**YAML frontmatter required** at file start:

```yaml
---
name: validate-order
description: Validates restaurant order data
display-name: Validate Order
keywords: [order, validate, menu, customer]
example-prompt:
  - Validate the order data in OrderController.cs
  - Can you check if this order request is valid?
  - Run validation on the customer's reservation order
---
```

| Attribute      | Format               | Example                                           |
|----------------|----------------------|---------------------------------------------------|
| name           | kebab-case           | `validate-order`                                  |
| description    | Full sentence        | Validates restaurant order data...                |
| display-name   | Title Case           | `Validate Order`                                  |
| keywords       | Lowercase array      | `[order, validate, menu]`                         |
| example-prompt | Array of 2-3 strings | Imperative + question styles, Restaurant examples |

## 6. Validation Checklist

### 6.1 SNIPPET Files

- [ ] Section numbers match IDEA exactly
- [ ] Gaps in numbering preserved (not renumbered)
- [ ] No boilerplate (copyright, using, namespace)
- [ ] Obvious constructors omitted
- [ ] Variable names descriptive
- [ ] No prohibited content per Section 1.1
- [ ] Implemented sections removed (verify against codebase)

### 6.2 IDEA Files

- [ ] No language specifics (C#, .NET)
- [ ] No ECS implementation terms
- [ ] CS concepts and architecture terms only
- [ ] No prohibited content per Section 1.1
- [ ] Requirements document structure followed
- [ ] Corresponding SNIPPET exists OR all features implemented (verify codebase)

### 6.3 SKILL Files

- [ ] YAML frontmatter present with `---` dividers
- [ ] All required attributes (name, description, display-name, keywords, example-prompt)
- [ ] Keywords lowercase in array format
- [ ] Name in kebab-case
- [ ] Example-prompt has 2-3 distinct examples (imperative + question styles)

### 6.4 RULE/SKILL Examples

- [ ] All examples use Restaurant application
- [ ] Paths follow `Restaurant.*` naming
- [ ] Domain concepts use Restaurant terminology
