# Project Audit Template

This template provides a standardized format for conducting comprehensive project audits across different aspects and projects.

## Universal Audit Prompt

Use this adaptable prompt to request auditing on any aspect of your project:

```
As a senior [ROLE], conduct a comprehensive [AUDIT_TYPE] audit of this project focusing on [FOCUS_AREA]. Please analyze:

[ANALYSIS_AREAS]

Create an audit report following the standardized template with:
- Executive summary with overall grade (A-F)
- Detailed analysis with specific file paths and line numbers where applicable
- Examples showing problems and solutions
- Severity ratings (🚨 Critical/❌ High/⚠️ Medium/ℹ️ Low)
- Prioritized action items with realistic timelines
- [READINESS_TYPE] assessment with specific recommendations

Be extremely detailed and nitpicky - include specific, actionable recommendations for every issue found.
```

## Audit Type Configurations

### **Code Quality Audit**
- **ROLE:** `software engineer`
- **AUDIT_TYPE:** `code quality`
- **FOCUS_AREA:** `production readiness and maintainability`
- **READINESS_TYPE:** `Production readiness`
- **ANALYSIS_AREAS:**
```
**Architecture & Design:**
- Class structure and inheritance patterns
- Design pattern implementation and violations
- SOLID principle adherence
- Coupling and cohesion analysis

**Implementation Quality:**
- Performance bottlenecks and optimization opportunities
- Thread safety and concurrency issues
- Error handling patterns and edge cases
- Resource management and disposal

**Code Quality:**
- Method complexity and code smells
- Naming conventions and consistency
- Magic numbers and hard-coded values
- Code duplication and maintainability
```

### **Asset Management Audit**
- **ROLE:** `technical artist/asset pipeline engineer`
- **AUDIT_TYPE:** `asset management`
- **FOCUS_AREA:** `scalable asset workflows and organization`
- **READINESS_TYPE:** `Production asset pipeline readiness`
- **ANALYSIS_AREAS:**
```
**Organization & Structure:**
- Directory hierarchy and categorization
- Naming conventions and consistency
- Asset metadata and configuration management
- Version control optimization

**Pipeline & Automation:**
- Asset build and export processes
- Quality assurance and validation
- Import/export automation
- Integration with development workflow

**Performance & Optimization:**
- Asset loading and memory usage
- Compression and optimization strategies
- Runtime performance impact
- Scalability considerations
```

### **System Architecture Audit**
- **ROLE:** `system architect`
- **AUDIT_TYPE:** `system architecture`
- **FOCUS_AREA:** `scalability, reliability, and maintainability`
- **READINESS_TYPE:** `Enterprise deployment readiness`
- **ANALYSIS_AREAS:**
```
**Architecture Design:**
- System component organization and boundaries
- Inter-service communication patterns
- Data flow and state management
- Scalability and performance architecture

**Infrastructure & Deployment:**
- Configuration management
- Environment consistency
- Monitoring and observability
- Disaster recovery and resilience

**Security & Compliance:**
- Authentication and authorization
- Data protection and privacy
- Security best practices
- Compliance requirements
```

### **Security Audit**
- **ROLE:** `security engineer`
- **AUDIT_TYPE:** `security`
- **FOCUS_AREA:** `vulnerability assessment and threat mitigation`
- **READINESS_TYPE:** `Security compliance readiness`
- **ANALYSIS_AREAS:**
```
**Application Security:**
- Input validation and sanitization
- Authentication and session management
- Authorization and access control
- Secure coding practices

**Infrastructure Security:**
- Network security configuration
- Data encryption and protection
- Dependency vulnerability assessment
- Security monitoring and logging

**Compliance & Governance:**
- Security policy adherence
- Audit trails and compliance
- Incident response procedures
- Security training and awareness
```

### **Performance Audit**
- **ROLE:** `performance engineer`
- **AUDIT_TYPE:** `performance`
- **FOCUS_AREA:** `optimization and scalability`
- **READINESS_TYPE:** `Performance scalability readiness`
- **ANALYSIS_AREAS:**
```
**Algorithm & Code Performance:**
- Computational complexity analysis
- Memory allocation patterns
- Hot path optimization opportunities
- Resource utilization efficiency

**System Performance:**
- Database query optimization
- Caching strategies and effectiveness
- Network communication efficiency
- Concurrency and parallelization

**Scalability Analysis:**
- Load testing and capacity planning
- Bottleneck identification
- Horizontal and vertical scaling options
- Performance monitoring and alerting
```

### **DevOps/Infrastructure Audit**
- **ROLE:** `DevOps engineer`
- **AUDIT_TYPE:** `DevOps infrastructure`
- **FOCUS_AREA:** `deployment reliability and operational excellence`
- **READINESS_TYPE:** `Operational readiness`
- **ANALYSIS_AREAS:**
```
**CI/CD Pipeline:**
- Build and deployment automation
- Testing integration and coverage
- Release management processes
- Environment promotion strategies

**Infrastructure Management:**
- Infrastructure as Code practices
- Configuration management
- Container and orchestration setup
- Resource provisioning and scaling

**Monitoring & Reliability:**
- Application and infrastructure monitoring
- Logging and observability
- Alerting and incident response
- Backup and disaster recovery
```

### **User Experience (UX) Audit**
- **ROLE:** `UX designer/researcher`
- **AUDIT_TYPE:** `user experience`
- **FOCUS_AREA:** `usability and user satisfaction`
- **READINESS_TYPE:** `User experience readiness`
- **ANALYSIS_AREAS:**
```
**Usability Assessment:**
- Interface design consistency
- Navigation and information architecture
- Accessibility compliance
- User workflow efficiency

**Performance & Responsiveness:**
- Load times and perceived performance
- Interactive feedback and animations
- Cross-device compatibility
- Error handling and user guidance

**User Research Integration:**
- User testing and feedback incorporation
- Analytics and behavior tracking
- Persona-based design validation
- Continuous improvement processes
```

## Report Template Structure

### Header Section
```markdown
# [Audit Type] Audit Report

**Project:** [Project Name]  
**Date:** [Date]  
**Reviewer:** [Reviewer Name/Role]  
**Review Scope:** [Scope Description]
```

### Executive Summary
```markdown
## Executive Summary

[Brief project assessment - 2-3 sentences]

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

### Detailed Analysis Section Template
```markdown
## Detailed Analysis

### 1. [Category Name]

#### **[Issue Number].[Subcategory] [Issue Title]**
**File:** `[FilePath:LineNumbers]`  
**Severity:** [🚨 CRITICAL | ❌ HIGH | ⚠️ MEDIUM | ℹ️ LOW]

```[language]
[Code snippet showing the problem]
```

**Problems:**
- **[Problem Type]** - [Description]
- **[Problem Type]** - [Description]

**Impact:** [Description of impact]

**Fix:**
```[language]
[Recommended solution code]
```
```

### Severity Rating Guidelines
- **🚨 CRITICAL:** Production blockers, security vulnerabilities, data corruption risks
- **❌ HIGH:** Major performance issues, architecture violations, maintainability problems
- **⚠️ MEDIUM:** Code quality issues, minor performance problems, inconsistencies
- **ℹ️ LOW:** Style issues, documentation gaps, minor improvements

### Priority Action Template
```markdown
## Priority Recommendations

### **Immediate (This Sprint):**
1. **[Action]** - [brief description]
2. **[Action]** - [brief description]

### **Short-term (Next 2 Weeks):**
1. **[Action]** - [brief description]
2. **[Action]** - [brief description]

### **Medium-term (Next Month):**
1. **[Action]** - [brief description]
2. **[Action]** - [brief description]
```

### Assessment Template
```markdown
## Production Readiness Assessment

**Current State:** [Ready/Not Ready] due to [key blockers]

**Required Work:** Approximately [time estimate] to address critical issues.

**Risk Areas:**
- [Risk 1 with impact description]
- [Risk 2 with impact description]

**Recommended Approach:** [Strategy for addressing issues]
```

## Quick Reference Examples

### Example 1: Asset Management Audit
```
As a senior technical artist/asset pipeline engineer, conduct a comprehensive asset management audit of this project focusing on scalable asset workflows and organization. Please analyze:

**Organization & Structure:**
- Directory hierarchy and categorization
- Naming conventions and consistency
- Asset metadata and configuration management
- Version control optimization

**Pipeline & Automation:**
- Asset build and export processes
- Quality assurance and validation
- Import/export automation
- Integration with development workflow

**Performance & Optimization:**
- Asset loading and memory usage
- Compression and optimization strategies
- Runtime performance impact
- Scalability considerations

Create an audit report following the standardized template with:
- Executive summary with overall grade (A-F)
- Detailed analysis with specific file paths and line numbers where applicable
- Examples showing problems and solutions
- Severity ratings (🚨 Critical/❌ High/⚠️ Medium/ℹ️ Low)
- Prioritized action items with realistic timelines
- Production asset pipeline readiness assessment with specific recommendations

Be extremely detailed and nitpicky - include specific, actionable recommendations for every issue found.
```

### Example 2: Security Audit
```
As a senior security engineer, conduct a comprehensive security audit of this project focusing on vulnerability assessment and threat mitigation. Please analyze:

**Application Security:**
- Input validation and sanitization
- Authentication and session management
- Authorization and access control
- Secure coding practices

**Infrastructure Security:**
- Network security configuration
- Data encryption and protection
- Dependency vulnerability assessment
- Security monitoring and logging

**Compliance & Governance:**
- Security policy adherence
- Audit trails and compliance
- Incident response procedures
- Security training and awareness

Create an audit report following the standardized template with:
- Executive summary with overall grade (A-F)
- Detailed analysis with specific file paths and line numbers where applicable
- Examples showing problems and solutions
- Severity ratings (🚨 Critical/❌ High/⚠️ Medium/ℹ️ Low)
- Prioritized action items with realistic timelines
- Security compliance readiness assessment with specific recommendations

Be extremely detailed and nitpicky - include specific, actionable recommendations for every issue found.
```

## Custom Audit Creation

To create a new audit type, define:

1. **ROLE:** The expert perspective (e.g., "database administrator", "mobile developer")
2. **AUDIT_TYPE:** The audit category (e.g., "database design", "mobile performance")
3. **FOCUS_AREA:** The main concern (e.g., "data integrity and query performance")
4. **READINESS_TYPE:** The outcome measure (e.g., "Database production readiness")
5. **ANALYSIS_AREAS:** Specific areas to examine

### Template for New Audit Types:
```
### **[New Audit Type] Audit**
- **ROLE:** `[expert role]`
- **AUDIT_TYPE:** `[audit category]`
- **FOCUS_AREA:** `[main focus]`
- **READINESS_TYPE:** `[readiness assessment type]`
- **ANALYSIS_AREAS:**
```
**[Category 1]:**
- [Specific area 1]
- [Specific area 2]
- [Specific area 3]

**[Category 2]:**
- [Specific area 1]
- [Specific area 2]
- [Specific area 3]
```
```

## Code Example Formatting Standards

### Problem Code:
```markdown
```csharp
// ❌ PROBLEM: Description of what's wrong
public void BadMethod()
{
    // Code with issues
}
```
```

### Solution Code:
```markdown
```csharp
// ✅ SOLUTION: Description of improvement
public void GoodMethod()
{
    // Improved code
}
```
```

## File Reference Standards

Always include exact file paths and line numbers:
- **File:** `Source/Project.Name/Folder/FileName.cs:45-67`
- **Files:** Multiple files listed with line numbers

## Metrics and Measurements Template

```markdown
## Metrics and Measurements

### **[Category] Metrics:**
- **[Metric Name]:** [Value] ([assessment])
- **[Metric Name]:** [Value] ([assessment])

### **Performance Considerations:**
- **[Performance Aspect]:** [Current state] ([needs/good])
- **[Performance Aspect]:** [Current state] ([needs/good])
```

## Usage Notes

1. **Be Specific:** Always include file paths, line numbers, and code examples
2. **Prioritize Issues:** Use severity ratings consistently
3. **Provide Solutions:** Don't just identify problems - show how to fix them
4. **Consider Context:** Tailor recommendations to the project's specific needs
5. **Focus on Impact:** Explain why each issue matters for production readiness

## Customization Guidelines

Adapt the template based on:
- **Project Type:** Web app, game, library, etc.
- **Technology Stack:** Language, frameworks, tools
- **Team Size:** Small team vs enterprise considerations
- **Production Environment:** Performance requirements, scale needs
- **Business Context:** Startup vs established product priorities