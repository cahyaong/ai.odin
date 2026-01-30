---
name: create-skill
description: Guides creation of SKILL files for AI agent interactions following established patterns and standards
display-name: Create Skill
keywords: [skill, create, template, agent, documentation, automation]
example-prompt:
  - Help me create a new skill for validating order data
  - Can you guide me through creating a SKILL file for menu management?
  - I want to create a skill that audits kitchen inventory
---

# SKILL: Create Skill

**Last Updated:** January 16, 2026

---

## Table of Contents

- [SKILL: Create Skill](#skill-create-skill)
  - [Table of Contents](#table-of-contents)
  - [1. Overview](#1-overview)
    - [1.1 What is a SKILL File](#11-what-is-a-skill-file)
    - [1.2 When to Create a New Skill](#12-when-to-create-a-new-skill)
  - [2. Required Components](#2-required-components)
    - [2.1 YAML Frontmatter](#21-yaml-frontmatter)
    - [2.2 Document Structure](#22-document-structure)
    - [2.3 Section Guidelines](#23-section-guidelines)
  - [3. Execution Workflow](#3-execution-workflow)
  - [4. Best Practices](#4-best-practices)
    - [4.1 Writing Execution Steps](#41-writing-execution-steps)
    - [4.2 Creating Checklists](#42-creating-checklists)
    - [4.3 Common Mistakes](#43-common-mistakes)
  - [5. Self-Assessment Protocol](#5-self-assessment-protocol)
  - [6. Output Template](#6-output-template)

---

Guides humans in creating SKILL files that define executable tasks for AI agents. Ensures consistency with `RULE_Document.md` and `RULE_Markdown.md` standards.

## 1. Overview

### 1.1 What is a SKILL File

A SKILL file defines a repeatable task that an AI agent can execute. It provides:

| Component            | Purpose                                          |
|----------------------|--------------------------------------------------|
| **YAML Frontmatter** | Metadata for discovery and invocation            |
| **Execution Steps**  | Clear workflow for the agent to follow           |
| **Self-Assessment**  | Verification checklist before presenting results |
| **Output Template**  | Expected format for results                      |

**Location:** All SKILL files reside in `.ai-toolbox/`

### 1.2 When to Create a New Skill

**Create a new skill when:**
- A task is repeatable and follows consistent steps
- Multiple humans would benefit from the same workflow
- The task requires specific domain knowledge or rules
- Output format should be standardized

**Do NOT create a skill when:**
- The task is one-time or highly specific
- Existing skills cover the use case
- The task is too simple (e.g., "read a file")
- The task requires no structured output

## 2. Required Components

### 2.1 YAML Frontmatter

**🚨 CRITICAL:** All SKILL files require YAML frontmatter at the top.

```yaml
---
name: validate-order
description: Validates restaurant order data against menu and inventory
display-name: Validate Order
keywords: [order, validate, menu, inventory, restaurant]
example-prompt:
  - Validate the order data in OrderController.cs
  - Can you check if this order request is valid?
  - Verify order items against the current menu
---
```

| Attribute        | Format           | Example                                        |
|------------------|------------------|------------------------------------------------|
| `name`           | kebab-case       | `validate-order`                               |
| `description`    | Full sentence    | Validates restaurant order data against menu   |
| `display-name`   | Title Case       | Validate Order                                 |
| `keywords`       | Lowercase array  | `[order, validate, menu]`                      |
| `example-prompt` | 2-3 string array | Distinct invocation examples                   |

### 2.2 Document Structure

**Required sections in order:**

| Section                     | Purpose                              | Required |
|-----------------------------|--------------------------------------|----------|
| YAML Frontmatter            | Discovery metadata                   | Yes      |
| H1 Title                    | `# SKILL: [Topic Name]`              | Yes      |
| Last Updated                | `**Last Updated:** Month DD, YYYY`   | Yes      |
| Table of Contents           | Full TOC with anchors                | Yes      |
| Brief Introduction          | One-liner after TOC                  | Yes      |
| Overview                    | What the skill does                  | Yes      |
| Execution Workflow          | Step-by-step process                 | Yes      |
| Self-Assessment Protocol    | Verification checklist               | Yes      |
| Output Template             | Expected result format               | Optional |

### 2.3 Section Guidelines

**H2 Sections:** Number sequentially (1, 2, 3...)

**H3 Subsections:** Number relative to parent (1.1, 1.2, 2.1...)

**Tables:** Use for structured data over verbose lists. See `RULE_Markdown.md` Section 4.3.

**Cross-References:** Link to related RULE and SKILL files when referencing external standards.

## 3. Execution Workflow

**Step 1: Gather Information**
1. Identify the repeatable task to automate
2. Document current manual process
3. List inputs, outputs, and decision points
4. Review existing SKILL files for patterns

**Step 2: Define Metadata**
1. Choose `name` (kebab-case, descriptive)
2. Write `description` (one full sentence)
3. Create `display-name` (Title Case)
4. Select 4-6 relevant `keywords` (lowercase)
5. Write 2-3 distinct `example-prompt` strings

**Step 3: Write Content**
1. Create brief introduction (one line)
2. Write Overview section explaining purpose
3. Document Execution Workflow with numbered steps
4. Add tables for structured information
5. Include cross-references to RULE files

**Step 4: Add Self-Assessment**
1. Create checklist with `🚨 CRITICAL` marker
2. Group related checks under bold headers
3. Cover completeness, accuracy, and formatting

**Step 5: Validate**
1. Run `SKILL_FormatMarkdown` on the file
2. Verify all required sections present
3. Confirm examples use Restaurant application
4. Check table column alignment

## 4. Best Practices

### 4.1 Writing Execution Steps

**Effective steps are:**

| Quality      | Description                          | Example                                        |
|--------------|--------------------------------------|------------------------------------------------|
| Specific     | Clear action to perform              | "Read the `OrderService.cs` file"              |
| Sequential   | Logical order                        | Validate → Process → Output                    |
| Conditional  | Handle decision points               | "If order invalid, report errors"              |
| Measurable   | Verifiable completion                | "List all validation failures with line numbers" |

**Example workflow for order validation:**
1. Load order data from specified file
2. Verify each item exists in `MenuService.cs`
3. Check quantity against `InventoryService.cs`
4. Validate customer payment method
5. Generate validation report

### 4.2 Creating Checklists

**Structure:**
```markdown
**🚨 CRITICAL:** Before presenting results, verify using this checklist:

**Order Validation:**
- [ ] All order items verified against menu
- [ ] Quantities checked against inventory
- [ ] Customer data validated

**Output Quality:**
- [ ] Report follows template format
- [ ] All errors include line numbers
- [ ] Recommendations are actionable
```

**Guidelines:**
- Group related items under bold headers
- Use checkbox format `- [ ]`
- Make each item verifiable (yes/no answer)
- Include both content and formatting checks

### 4.3 Common Mistakes

| Mistake                    | Problem                              | Fix                                            |
|----------------------------|--------------------------------------|------------------------------------------------|
| Missing YAML frontmatter   | Skill not discoverable               | Add all required attributes                    |
| Generic keywords           | Poor search results                  | Use domain-specific terms                      |
| No self-assessment         | Quality not verified                 | Add checklist section                          |
| Missing cross-references   | Context lost                         | Link to RULE and related SKILL files           |
| Non-Restaurant examples    | Violates `.ai-toolbox/` requirement  | Use Order, Menu, Kitchen examples              |
| Vague execution steps      | Agent cannot follow                  | Make steps specific and actionable             |

## 5. Self-Assessment Protocol

**🚨 CRITICAL:** Before presenting results, verify using this checklist:

**YAML Frontmatter:**
- [ ] All required attributes present (`name`, `description`, `display-name`, `keywords`, `example-prompt`)
- [ ] `name` uses kebab-case format
- [ ] `keywords` are lowercase array
- [ ] `example-prompt` has 2-3 distinct examples
- [ ] `description` is a complete sentence

**Document Structure:**
- [ ] H1 follows `# SKILL: [Topic]` format
- [ ] Last Updated date present and current
- [ ] TOC includes all H2 and H3 sections
- [ ] First TOC entry matches H1 anchor
- [ ] Brief introduction after TOC divider

**Required Sections:**
- [ ] Overview section explains skill purpose
- [ ] Execution Workflow has numbered steps
- [ ] Self-Assessment Protocol section present
- [ ] Output Template or Report format included

**Content Quality:**
- [ ] All examples use Restaurant application (Order, Menu, Kitchen)
- [ ] Tables used for structured data
- [ ] Cross-references to RULE files included
- [ ] Steps are specific and actionable

**Formatting Compliance:**
- [ ] Run `SKILL_FormatMarkdown` validation
- [ ] Table columns properly aligned
- [ ] Heading numbers sequential
- [ ] No prohibited content per `RULE_Document.md` Section 1.2

## 6. Output Template

Use this template when creating new SKILL files:

```markdown
---
name: [kebab-case-name]
description: [Full sentence describing what the skill does]
display-name: [Title Case Name]
keywords: [keyword1, keyword2, keyword3, keyword4]
example-prompt:
  - [First example invocation]
  - [Second example invocation]
  - [Third example invocation]
---

# SKILL: [Topic Name]

**Last Updated:** [Month DD, YYYY]

---

## Table of Contents

- [SKILL: [Topic Name]](#skill-topic-name)
  - [Table of Contents](#table-of-contents)
  - [1. Overview](#1-overview)
  - [2. Execution Workflow](#2-execution-workflow)
  - [3. Self-Assessment Protocol](#3-self-assessment-protocol)
  - [4. Output Template](#4-output-template)

---

[One-line description of what this skill does and its purpose.]

## 1. Overview

[Explain the skill's purpose, when to use it, and key concepts.]

## 2. Execution Workflow

1. [First step]
2. [Second step]
3. [Third step]

## 3. Self-Assessment Protocol

**🚨 CRITICAL:** Before presenting results, verify using this checklist:

**[Category 1]:**
- [ ] [Verification item]
- [ ] [Verification item]

**[Category 2]:**
- [ ] [Verification item]
- [ ] [Verification item]

## 4. Output Template

[Expected format for skill output]
```
