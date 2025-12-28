---
name: audit-codebase
description: Conducts comprehensive codebase audits with configurable personas (Code Quality, Security, Performance, Architecture, Asset Management, DevOps, UX), generates detailed AUDIT_*.md reports with severity ratings, prioritized recommendations, and historical tracking
display-name: Audit Codebase
keywords: [audit, codebase, quality, assessment, security, performance, architecture, analysis, report, evaluation]
---

# Audit Codebase

**Last Updated:** December 26, 2025

Conducts comprehensive codebase audits using role-specific perspectives to assess code quality, architecture, performance, security, and more. Generates detailed AUDIT reports with severity ratings, actionable recommendations, and historical tracking.

## 1. Overview

### 1.1 Purpose

This skill enables systematic codebase evaluation through expert personas, producing detailed audit reports that identify issues, prioritize improvements, and track progress over time.

### 1.2 Key Capabilities

- **Full codebase scanning** - Analyzes entire `Source/` directory recursively
- **Persona-driven analysis** - Adopts specific expert perspectives (engineer, architect, security specialist)
- **Dual-depth reporting** - Combines high-level overview with detailed deep-dive analysis
- **Severity classification** - Uses consistent priority ratings (🚨 CRITICAL, ❌ HIGH, ⚠️ MEDIUM, ℹ️ LOW)
- **Actionable recommendations** - Provides specific file paths, line numbers, code examples, and solutions
- **Historical tracking** - Archives previous audits to measure improvement over time
- **Auto-validation** - Ensures generated reports comply with documentation standards

### 1.3 Output Location

Generated audit reports are saved in `.ai-context/AUDIT_*.md` format where `*` indicates the audit type (e.g., `AUDIT_CodeQuality.md`, `AUDIT_Performance.md`).

## 2. Audit Types

This skill supports 16 persona-based audit types. For complete persona definitions including expertise areas and readiness criteria, see `RULE_Persona.md`.

**Available Audit Types:**

**General Software:**
- **Code Quality** - Software Engineer perspective on production readiness and maintainability
- **System Architecture** - System Architect perspective on scalability, reliability, and system design
- **Performance** - Performance Engineer perspective on optimization and scalability
- **Security** - Security Engineer perspective on vulnerability assessment and threat mitigation
- **Asset Management** - Technical Artist/Asset Pipeline Engineer perspective on asset workflows
- **DevOps Infrastructure** - DevOps Engineer perspective on deployment reliability and operations
- **User Experience** - UX Designer/Researcher perspective on usability and satisfaction

**Game-Specific:**
- **Game Design** - Game Designer perspective on gameplay mechanics, systems, and player experience
- **Level Design** - Level Designer perspective on spatial design and environmental storytelling
- **Narrative Design** - Narrative Designer/Game Writer perspective on story, dialogue, and narrative systems
- **Audio Design** - Audio Designer/Sound Engineer perspective on music, sound effects, and audio implementation
- **QA Testing** - Game Tester/QA Specialist perspective on quality assurance and gameplay testing

**Documentation-Specific:**
- **Technical Writing** - Technical Writer perspective on documentation clarity, accuracy, and usability
- **Information Architecture** - Information Architect perspective on content organization, findability, and structure

**Research-Specific:**
- **ML Research** - ML Research Scientist perspective on machine learning algorithms, experiment design, and validation
- **Data Science** - Data Scientist perspective on data analysis, experimentation, and insight generation

## 3. Execution Workflow

When invoked to conduct a codebase audit, follow this systematic process:

### 3.1 Determine Audit Type

If audit type not specified by user:
1. Present available audit types (Code Quality, System Architecture, Performance, Security, Asset Management, DevOps Infrastructure, User Experience)
2. Ask user to select desired audit focus
3. Confirm selection before proceeding

### 3.2 Full Codebase Scanning

Systematically analyze the entire `Source/` directory:
1. **Recursive file discovery** - Identify all source files (.cs, .csproj, .yaml, .json, configuration files)
2. **Project structure mapping** - Understand solution organization, project dependencies, namespace hierarchy
3. **Component identification** - Catalog classes, interfaces, systems, components, and their relationships

### 3.3 High-Level Analysis

Conduct architectural and trend analysis:
1. **Architectural patterns** - Identify design patterns, architectural style (ECS, MVC, layered), separation of concerns
2. **System-wide trends** - Common code smells, recurring anti-patterns, consistency issues
3. **Dependency analysis** - External dependencies, internal coupling, circular dependencies
4. **Overall health metrics** - Code complexity distribution, test coverage patterns, documentation quality

### 3.4 Deep-Dive Analysis

Perform detailed file-by-file examination:
1. **Issue identification** - Specific violations, performance problems, security vulnerabilities, design flaws
2. **Location precision** - Exact file paths and line number ranges
3. **Code extraction** - Extract relevant code snippets showing problems
4. **Impact assessment** - Evaluate severity and consequences of each issue
5. **Solution formulation** - Provide corrected code examples and implementation guidance

### 3.5 Apply Severity Ratings

Classify each issue using consistent severity levels:
- **🚨 CRITICAL** - Production blockers, security vulnerabilities, data corruption risks
- **❌ HIGH** - Major performance issues, architecture violations, maintainability problems
- **⚠️ MEDIUM** - Code quality issues, minor performance problems, inconsistencies
- **ℹ️ LOW** - Style issues, documentation gaps, minor improvements

### 3.6 Generate Prioritized Recommendations

Create actionable improvement plan:
1. **Immediate Actions** (This Sprint) - Address critical issues blocking production
2. **Short-term Improvements** (Next 2 Weeks) - Resolve high-priority problems
3. **Medium-term Enhancements** (Next Month) - Improve quality and maintainability

### 3.7 Handle Historical Audits

Check for existing audit file:
1. If `AUDIT_[Type].md` exists in `.ai-context/`
2. Read entire content of existing audit
3. Move to Appendix section with timestamp header
4. Format as: `### Audit from [Month DD, YYYY]`
5. Order from latest to oldest in Appendix

### 3.8 Create Audit Report

Generate new `AUDIT_*.md` file:
1. Use report structure template (Section 4)
2. Ensure high-level content before detailed analysis
3. Include all required sections
4. Add Appendix with historical audits if applicable
5. Save to `.ai-context/AUDIT_[Type].md`

### 3.9 Auto-Validation

Use SKILL_FormatMarkdown to validate report:
1. Invoke Format Markdown skill on generated audit file
2. Check compliance with RULE_Markdown.md standards
3. Apply any formatting corrections automatically
4. Verify 100% compliance before completion

### 3.10 Self-Assessment Protocol

Before presenting results, verify quality:
1. Use Self-Assessment Checklist (Section 5)
2. Confirm all requirements met
3. Validate severity ratings are appropriate
4. Ensure recommendations are actionable
5. Only report completion after 100% verification

## 4. Report Structure Template

Organize audit reports from general to specific using this standardized structure:

### 4.1 Header Section

```markdown
# [Audit Type] Audit Report

**Project:** [Project Name]  
**Date:** [Month DD, YYYY]  
**Reviewer:** [Role/Perspective]  
**Review Scope:** Full codebase analysis
```

### 4.2 Executive Summary

```markdown
## Executive Summary

[Brief 2-3 sentence assessment of overall codebase health]

**Overall Grade: [A-F]**

### Key Strengths
- ✅ [Strength 1]
- ✅ [Strength 2]
- ✅ [Strength 3]

### Critical Issues Requiring Attention
- 🚨 **CRITICAL:** [Critical Issue 1]
- 🚨 **CRITICAL:** [Critical Issue 2]
- ❌ **HIGH:** [High Priority Issue 1]
- ❌ **HIGH:** [High Priority Issue 2]
```

### 4.3 High-Level Analysis

```markdown
## High-Level Analysis

### Architectural Patterns
[Description of architectural style, design patterns, overall structure]

### System-Wide Trends
[Common patterns, recurring issues, consistency observations]

### Dependency Analysis
[External dependencies, internal coupling, circular references]

### Overall Metrics
[Code complexity, test coverage, documentation quality]
```

### 4.4 Detailed Analysis

```markdown
## Detailed Analysis

### 1. [Category Name]

#### **1.1 [Issue Title]**
**File:** `[FilePath:LineNumbers]`  
**Severity:** [🚨 CRITICAL | ❌ HIGH | ⚠️ MEDIUM | ℹ️ LOW]

```[language]
[Code snippet showing the problem]
```

**Problems:**
- **[Problem Type]** - [Description]
- **[Problem Type]** - [Description]

**Impact:** [Description of consequences]

**Fix:**
```[language]
[Recommended solution code]
```
```

### 4.5 Priority Recommendations

```markdown
## Priority Recommendations

### Immediate Actions (This Sprint)
1. **[Action]** - [Brief description and rationale]
2. **[Action]** - [Brief description and rationale]

### Short-term Improvements (Next 2 Weeks)
1. **[Action]** - [Brief description and rationale]
2. **[Action]** - [Brief description and rationale]

### Medium-term Enhancements (Next Month)
1. **[Action]** - [Brief description and rationale]
2. **[Action]** - [Brief description and rationale]
```

### 4.6 Metrics & Measurements

```markdown
## Metrics and Measurements

### Code Quality Metrics
- **[Metric Name]:** [Value] ([assessment: good/needs improvement])
- **[Metric Name]:** [Value] ([assessment: good/needs improvement])

### Performance Metrics
- **[Metric Name]:** [Value] ([assessment: acceptable/concerning])
- **[Metric Name]:** [Value] ([assessment: acceptable/concerning])
```

### 4.7 Production Readiness Assessment

```markdown
## Production Readiness Assessment

**Current State:** [Ready/Not Ready] due to [key blockers]

**Required Work:** Approximately [time estimate] to address critical issues.

**Risk Areas:**
- [Risk 1 with impact description]
- [Risk 2 with impact description]

**Recommended Approach:** [Strategy for addressing issues]

**Success Metrics:**
- [Specific measurable goal 1]
- [Specific measurable goal 2]
```

### 4.8 Historical Tracking (Appendix)

```markdown
## Appendix: Historical Audits

### Audit from [Month DD, YYYY]

[Complete content of previous audit report]

### Audit from [Month DD, YYYY]

[Complete content of earlier audit report]
```

## 5. Self-Assessment Protocol

**CRITICAL:** Before presenting audit results, systematically verify quality using this checklist:

### 5.1 Completeness Verification

- [ ] Executive summary includes overall grade and key issues
- [ ] High-level analysis covers architectural patterns and trends
- [ ] Detailed analysis includes specific file paths and line numbers
- [ ] All severity ratings assigned consistently
- [ ] Priority recommendations organized by timeframe
- [ ] Production readiness assessment provided
- [ ] Historical audits moved to Appendix if applicable

### 5.2 Severity Rating Accuracy

- [ ] 🚨 CRITICAL used only for production blockers, security vulnerabilities, data corruption
- [ ] ❌ HIGH used for major performance issues, architecture violations, maintainability problems
- [ ] ⚠️ MEDIUM used for code quality issues, minor performance problems, inconsistencies
- [ ] ℹ️ LOW used for style issues, documentation gaps, minor improvements
- [ ] Each severity level has clear justification

### 5.3 Actionable Recommendations

- [ ] Each recommendation has specific action description
- [ ] File paths are accurate and complete
- [ ] Line numbers point to correct code locations
- [ ] Code examples show both problem and solution
- [ ] Solutions are practical and implementable
- [ ] Timeframe estimates are realistic

### 5.4 Code Example Quality

- [ ] Problem code snippets are complete and compilable
- [ ] Solution code snippets demonstrate fixes clearly
- [ ] Code examples use proper syntax highlighting
- [ ] Examples include sufficient context
- [ ] No truncated code with ellipsis comments

### 5.5 Format Compliance

- [ ] YAML metadata present with required fields (name, description, display-name, keywords)
- [ ] Last Updated date in correct format (Month DD, YYYY)
- [ ] Heading numbering sequential (H2: 1, 2, 3...; H3: 1.1, 1.2, 1.3...)
- [ ] Folder paths end with forward slash `/`
- [ ] File paths have no trailing slash
- [ ] Emojis followed by UPPERCASE text (✅ CORRECT, ❌ WRONG)
- [ ] No code identifiers in headings
- [ ] Historical audits formatted correctly in Appendix

### 5.6 Final Validation

- [ ] Run SKILL_FormatMarkdown on generated audit file
- [ ] Apply all formatting corrections
- [ ] Confirm 100% compliance with RULE_Markdown.md
- [ ] Verify audit file saved in `.ai-context/` directory
- [ ] Re-read generated file to confirm accuracy

## 6. Historical Tracking

### 6.1 Archive Format

When creating new audit and previous audit exists:

```markdown
## Appendix: Historical Audits

### Audit from December 26, 2025

[Complete previous audit content preserved exactly as written]

### Audit from November 15, 2025

[Even older audit content if exists]
```

### 6.2 Comparison Guidelines

When analyzing improvement over time:
- Note resolved issues from previous audits
- Highlight newly introduced problems
- Track grade progression (e.g., C → B → A)
- Document architectural evolution
- Measure metric improvements

### 6.3 Timestamp Format

Use consistent date format for historical audits:
- Format: `### Audit from Month DD, YYYY`
- Example: `### Audit from December 26, 2025`
- Order: Latest to oldest (most recent first)

## 7. Usage Examples

### 7.1 Code Quality Audit

**User Request:**
```
Use the "Audit Codebase" skill to conduct a code quality audit
```

**Agent Response:**
1. Confirms audit type: "Conducting Code Quality audit with senior software engineer perspective"
2. Scans entire `Source/` directory recursively
3. Analyzes architecture, implementation quality, code quality
4. Identifies critical issues (thread safety, performance bottlenecks)
5. Generates `.ai-context/AUDIT_CodeQuality.md` with detailed findings
6. Validates report format with SKILL_FormatMarkdown
7. Performs self-assessment verification
8. Presents summary: "Code Quality audit complete. Overall Grade: B-. Found 2 CRITICAL, 4 HIGH, 8 MEDIUM issues. Report saved to .ai-context/AUDIT_CodeQuality.md"

### 7.2 Performance Audit

**User Request:**
```
I need a performance assessment of the codebase
```

**Agent Response:**
1. Asks: "I can conduct a Performance audit. Shall I proceed with performance engineer perspective focusing on optimization and scalability?"
2. User confirms
3. Analyzes algorithm complexity, system performance, scalability
4. Checks for existing `AUDIT_Performance.md`
5. If exists: Moves previous audit to Appendix with timestamp
6. Generates new `.ai-context/AUDIT_Performance.md`
7. Auto-validates and performs self-assessment
8. Reports: "Performance audit complete. Overall Grade: C+. Key bottleneck identified in entity querying (O(n*m) complexity). Report saved with previous audit archived in Appendix."

### 7.3 Security Audit

**User Request:**
```
Audit codebase
```

**Agent Response:**
1. Asks: "Which type of audit would you like me to conduct? Available options: Code Quality, System Architecture, Performance, Security, Asset Management, DevOps Infrastructure, User Experience"
2. User selects: "Security"
3. Conducts security audit with security engineer perspective
4. Examines application security, infrastructure security, compliance
5. Identifies vulnerabilities and security risks
6. Generates `.ai-context/AUDIT_Security.md`
7. Validates and self-assesses
8. Presents results with severity breakdown

### 7.4 Asset Management Audit

**User Request:**
```
Use the "Audit Codebase" skill to evaluate our asset pipeline
```

**Agent Response:**
1. Conducts Asset Management audit with technical artist perspective
2. Analyzes asset organization, pipeline automation, optimization strategies
3. Evaluates directory structure, naming conventions, build processes
4. Checks memory usage patterns and compression strategies
5. Generates `.ai-context/AUDIT_AssetManagement.md`
6. Archives previous asset audit in Appendix if exists
7. Completes validation and self-assessment
8. Reports findings with actionable recommendations

## 8. Notes

### 8.1 Best Practices

- **Be thorough and detailed** - Include specific examples, not just high-level observations
- **Use precise locations** - Always provide file paths and line numbers for issues
- **Show solutions** - Don't just identify problems, demonstrate how to fix them
- **Prioritize effectively** - Use severity ratings consistently to guide improvement efforts
- **Track progress** - Archive historical audits to measure improvement over time

### 8.2 Integration with Other Skills

- **SKILL_FormatMarkdown** - Automatically validates generated audit reports
- **AUDIT reports** - Generate standardized documentation in `.ai-context/`
- **ROADMAP documents** - Audit findings can inform feature planning priorities

### 8.3 Customization Guidelines

Adapt audit focus based on:
- Project type (game engine, web application, library)
- Technology stack (.NET, Godot, specific frameworks)
- Team size (solo developer vs enterprise team)
- Production environment (performance requirements, scale)
- Business context (startup rapid iteration vs established product)

### 8.4 Quality Standards

All generated audit reports must:
- Follow RULE_Markdown.md formatting standards completely
- Include YAML metadata in SKILL files (not in AUDIT reports)
- Use consistent severity emoji indicators
- Provide actionable recommendations with timeframes
- Archive historical audits properly in Appendix
- Pass SKILL_FormatMarkdown validation 100%
