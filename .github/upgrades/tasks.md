# .NET 6.0 ? 10.0 Upgrade Execution Tasks

**Scenario**: All-At-Once .NET 10.0 Upgrade  
**Strategy**: Atomic upgrade of all 13 projects simultaneously  
**Branch**: upgrade-to-NET10  
**Target**: net10.0  
**Status**: Ready for execution

---
**Progress**: 9/10 tasks complete (90%) ![90%](https://progress-bar.xyz/90)
## Task Execution Status Dashboard

| Task | Status | Description | Effort |
|------|--------|-------------|--------|
| TASK-001 | [?] | Prerequisites & Environment Validation | Low |
| TASK-002 | [?] | Update All Project Files to net10.0 | Low |
| TASK-003 | [?] | Update NuGet Packages (16 packages) | Low |
| TASK-004 | [?] | Remove Deprecated Packages (5 packages) | Low |
| TASK-005 | [?] | Add System.Drawing.Common NuGet Package | Low |
| TASK-006 | [?] | Restore & Build Solution | Medium |
| TASK-007 | [?] | Fix API Compatibility Issues | Medium |
| TASK-008 | [?] | Run Unit Tests | Low |
| TASK-009 | [?] | Manual Integration Testing | Medium |
| TASK-010 | [ ] | Commit Changes | Low |

---

## Detailed Tasks

### [?] TASK-001: Prerequisites & Environment Validation *(Completed: 2026-01-19 20:58)*
**Effort**: Low | **Duration**: 5-10 minutes  
**Branch**: upgrade-to-NET10  
**Objective**: Verify environment is ready for upgrade

#### Actions

- [?] (1) Validate .NET 10.0 SDK Installation
  **Instruction**: Run `dotnet --version` and verify output shows 10.0.x or higher
  **Expected**: SDK version 10.0.0 or later
  **Validation**: ? Success if version ? 10.0.0

- [?] (2) Check global.json (if present)
  **Instruction**: If `global.json` exists in repo root, verify it won't block SDK
  **Expected**: No incompatibilities with .NET 10.0
  **Validation**: ? No modifications needed OR ? Updated if necessary

- [?] (3) Verify Repository State
  **Instruction**: Confirm on upgrade-to-NET10 branch with clean working directory
  **Command**: `git status` ? should show "working tree clean"
  **Expected**: No uncommitted changes
  **Validation**: ? Clean working directory confirmed

- [?] (4) Backup Current State
  **Instruction**: Document commit hash of current state as fallback point
  **Command**: `git log -1 --oneline`
  **Expected**: Current commit documented
  **Validation**: ? Hash recorded

#### Success Criteria
- ? .NET 10.0 SDK available
- ? Repository clean
- ? Backup point documented

---

### [?] TASK-002: Update All Project Files to net10.0 *(Completed: 2026-01-19 21:01)*
**Effort**: Low | **Duration**: 5-10 minutes  
**Objective**: Update TargetFramework in all 13 project files

#### Projects to Update

```
Foundation (4 projects):
  1. src/BCKFreightTMS.Common/BCKFreightTMS.Common.csproj
  2. src/Data/BCKFreightTMS.Data.Common/BCKFreightTMS.Data.Common.csproj
  3. src/Services/BCKFreightTMS.Services.Mapping/BCKFreightTMS.Services.Mapping.csproj
  4. src/Services/BCKFreightTMS.Services.Messaging/BCKFreightTMS.Services.Messaging.csproj

Data Layer (2 projects):
  5. src/Data/BCKFreightTMS.Data.Models/BCKFreightTMS.Data.Models.csproj
  6. src/Data/BCKFreightTMS.Data/BCKFreightTMS.Data.csproj

Business Logic (2 projects):
  7. src/Services/BCKFreightTMS.Services/BCKFreightTMS.Services.csproj
  8. src/Services/BCKFreightTMS.Services.Data/BCKFreightTMS.Services.Data.csproj

Presentation (3 projects):
  9. src/Web/BCKFreightTMS.Web/BCKFreightTMS.Web.csproj
  10. src/Web/BCKFreightTMS.Web.ViewModels/BCKFreightTMS.Web.ViewModels.csproj
  11. src/Web/BCKFreightTMS.Web.Tests/BCKFreightTMS.Web.Tests.csproj

Tests (2 projects):
  12. src/Tests/BCKFreightTMS.Services.Data.Tests/BCKFreightTMS.Services.Data.Tests.csproj
  13. src/Tests/Sandbox/Sandbox.csproj
```

#### Actions

- [?] (1) Update TargetFramework in All Projects
  **Instruction**: In each .csproj file, change:
  ```xml
  <TargetFramework>net6.0</TargetFramework>
  ```
  to:
  ```xml
  <TargetFramework>net10.0</TargetFramework>
  ```
  **Method**: Edit each of 13 project files
  **Expected**: All projects updated to net10.0
  **Validation**: ? All 13 files modified, verify with: `grep -r "net10.0" src/`

- [?] (2) Verify All Projects Updated
  **Instruction**: Run dotnet command to list target frameworks
  **Command**: `dotnet list --format json 2>/dev/null | grep -i targetframework` or visual inspection
  **Expected**: All projects show net10.0
  **Validation**: ? All 13 projects target net10.0

#### Success Criteria
- ? All 13 project files updated
- ? No projects remain on net6.0
- ? Changes saved

---

### [?] TASK-003: Update NuGet Packages (16 packages) *(Completed: 2026-01-19 21:04)*
**Effort**: Low | **Duration**: 10-15 minutes  
**Objective**: Upgrade 16 NuGet packages to version 10.0.2

#### Packages to Update (All to version 10.0.2)

**ASP.NET Core & Identity (8 packages)**:
```
1. Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
2. Microsoft.AspNetCore.Identity.EntityFrameworkCore
3. Microsoft.AspNetCore.Identity.UI
4. Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation
5. Microsoft.AspNetCore.Mvc.Testing
6. Microsoft.VisualStudio.Web.CodeGeneration.Design
```

**Entity Framework Core (7 packages)**:
```
7. Microsoft.EntityFrameworkCore.Design
8. Microsoft.EntityFrameworkCore.InMemory
9. Microsoft.EntityFrameworkCore.Proxies
10. Microsoft.EntityFrameworkCore.SqlServer
11. Microsoft.EntityFrameworkCore.Tools
```

**Microsoft.Extensions (6 packages - Sandbox)**:
```
12. Microsoft.Extensions.Configuration
13. Microsoft.Extensions.Configuration.EnvironmentVariables
14. Microsoft.Extensions.Configuration.Json
15. Microsoft.Extensions.Configuration.UserSecrets
16. Microsoft.Extensions.Logging.Console
```

#### Actions

- [?] (1) Update Packages via dotnet CLI (Bulk Method)
  **Instruction**: Run commands to update all packages
  **Commands**:
  ```bash
  dotnet package update Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore --version 10.0.2
  dotnet package update Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 10.0.2
  dotnet package update Microsoft.AspNetCore.Identity.UI --version 10.0.2
  dotnet package update Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation --version 10.0.2
  dotnet package update Microsoft.AspNetCore.Mvc.Testing --version 10.0.2
  dotnet package update Microsoft.VisualStudio.Web.CodeGeneration.Design --version 10.0.2
  dotnet package update Microsoft.EntityFrameworkCore.Design --version 10.0.2
  dotnet package update Microsoft.EntityFrameworkCore.InMemory --version 10.0.2
  dotnet package update Microsoft.EntityFrameworkCore.Proxies --version 10.0.2
  dotnet package update Microsoft.EntityFrameworkCore.SqlServer --version 10.0.2
  dotnet package update Microsoft.EntityFrameworkCore.Tools --version 10.0.2
  dotnet package update Microsoft.Extensions.Configuration --version 10.0.2
  dotnet package update Microsoft.Extensions.Configuration.EnvironmentVariables --version 10.0.2
  dotnet package update Microsoft.Extensions.Configuration.Json --version 10.0.2
  dotnet package update Microsoft.Extensions.Configuration.UserSecrets --version 10.0.2
  dotnet package update Microsoft.Extensions.Logging.Console --version 10.0.2
  ```
  **Expected**: All 16 packages updated to 10.0.2
  **Validation**: ? Package versions verified in .csproj files

- [?] (2) Verify All Package Updates
  **Instruction**: Inspect affected .csproj files to confirm versions
  **Method**: Check each file for `Version="10.0.2"` entries
  **Expected**: All 16 packages show 10.0.2
  **Validation**: ? All versions confirmed

#### Success Criteria
- ? 16 packages updated to 10.0.2
- ? No version conflicts
- ? All files modified

---

### [?] TASK-004: Remove Deprecated Packages (5 packages) *(Completed: 2026-01-19 21:05)*
**Effort**: Low | **Duration**: 5-10 minutes  
**Objective**: Remove 5 packages now included in .NET 10.0 framework

#### Packages to Remove

```
1. Microsoft.AspNetCore.Http (2.3.9)
   Projects: Services, Services.Data, Services.Data.Tests, Web, Web.Tests, Web.ViewModels, Sandbox

2. Microsoft.AspNetCore.Http.Abstractions (2.3.9)
   Projects: Data, Services, Services.Data, Services.Data.Tests, Web, Web.Tests, Web.ViewModels, Sandbox

3. System.ComponentModel.Annotations (5.0.0.0)
   Projects: Data.Common

4. (Implicit removals - already covered above)
```

#### Actions

- [?] (1) Remove Package References from .csproj Files
  **Instruction**: Delete `<PackageReference>` lines for deprecated packages from all affected .csproj files
  **Method**: Edit each affected project file and remove the PackageReference element
  **Pattern to Remove**:
  ```xml
  <PackageReference Include="Microsoft.AspNetCore.Http" Version="2.3.9" />
  <PackageReference Include="Microsoft.AspNetCore.Http.Abstractions" Version="2.3.9" />
  <PackageReference Include="System.ComponentModel.Annotations" Version="5.0.0.0" />
  ```
  **Expected**: References removed from all projects
  **Validation**: ? Lines removed, files saved

- [?] (2) Verify Removal
  **Instruction**: Search for deprecated package references
  **Command**: `grep -r "Microsoft.AspNetCore.Http\"" src/` ? should be empty
  **Expected**: No deprecated package references found
  **Validation**: ? No references remain

#### Success Criteria
- ? 5 package references removed
- ? No compilation errors from removal
- ? All affected projects updated

---

### [?] TASK-005: Add System.Drawing.Common NuGet Package *(Completed: 2026-01-19 21:05)*
**Effort**: Low | **Duration**: 5 minutes  
**Objective**: Add System.Drawing.Common package for image/PDF operations

#### Actions

- [?] (1) Add System.Drawing.Common to BCKFreightTMS.Services
  **Instruction**: Add NuGet package reference to Services.csproj
  **Package**: System.Drawing.Common (version 6.0.0 for compatibility)
  **XML to Add**:
  ```xml
  <PackageReference Include="System.Drawing.Common" Version="6.0.0" />
  ```
  **File**: src/Services/BCKFreightTMS.Services/BCKFreightTMS.Services.csproj
  **Expected**: Package reference added
  **Validation**: ? Reference visible in .csproj

- [?] (2) Add System.Drawing.Common to BCKFreightTMS.Web
  **Instruction**: Add same package reference to Web.csproj
  **File**: src/Web/BCKFreightTMS.Web/BCKFreightTMS.Web.csproj
  **Expected**: Package reference added to Web project
  **Validation**: ? Reference visible in .csproj

- [?] (3) Verify Packages Added
  **Instruction**: Confirm both projects now reference System.Drawing.Common
  **Command**: `grep -r "System.Drawing.Common" src/`
  **Expected**: References in Services and Web projects
  **Validation**: ? Both references confirmed

#### Success Criteria
- ? System.Drawing.Common added to Services
- ? System.Drawing.Common added to Web
- ? Version 6.0.0 specified

---

### [?] TASK-006: Restore & Build Solution *(Completed: 2026-01-19 21:08)*
**Effort**: Medium | **Duration**: 10-20 minutes  
**Objective**: Restore dependencies and attempt initial build to identify breaking changes

#### Actions

- [?] (1) Clean Solution
  **Instruction**: Remove build artifacts and cache
  **Command**: `dotnet clean`
  **Expected**: Solution cleaned
  **Validation**: ? Clean completed without errors

- [?] (2) Restore NuGet Dependencies
  **Instruction**: Restore all NuGet packages for net10.0
  **Command**: `dotnet restore`
  **Expected**: All dependencies restored successfully
  **Validation**: ? Restore completed, no missing dependencies
  **If Failed**: Check for package version conflicts, review plan Package Reference section

- [?] (3) Build Solution (Release Configuration)
  **Instruction**: Build entire solution to identify compilation errors
  **Command**: `dotnet build --configuration Release`
  **Expected**: Initial build may show errors - this is normal
  **Validation**: Document all error messages
  **If Errors**: Proceed to TASK-007 for API compatibility fixes

- [?] (4) Document Compilation Errors
  **Instruction**: If build errors occur, save error list
  **Action**: Review errors from:
    - System.Drawing API usage
    - Hosting model changes
    - Identity configuration changes
    - Other breaking changes
  **Expected**: Errors documented for TASK-007
  **Validation**: ? Error list created

#### Success Criteria
- ? Dependencies restored
- ? Build completed (errors OK at this stage)
- ? Errors documented

---

### [?] TASK-007: Fix API Compatibility Issues *(Completed: 2026-01-19 21:11)*
**Effort**: Medium | **Duration**: 20-40 minutes  
**Objective**: Address compilation errors and breaking changes identified in TASK-006

#### Actions

- [?] (1) Fix System.Drawing Issues (BCKFreightTMS.Services)
  **Instruction**: Review Services project for System.Drawing usage
  **Files to Check**: src/Services/BCKFreightTMS.Services/ (all .cs files)
  **Expected Issues**: Bitmap, Image, ImageCodecInfo, Encoder usage
  **Action**: Since System.Drawing.Common package added, most issues should resolve
  **If Still Failing**: Check that package reference is correct
  **Validation**: ? Specific classes resolved or ? Code patterns updated

- [?] (2) Fix System.Drawing Issues (BCKFreightTMS.Web)
  **Instruction**: Review Web project for System.Drawing usage
  **Files to Check**: src/Web/BCKFreightTMS.Web/ (especially image handling code)
  **Expected Issues**: Thumbnail generation, image operations
  **Action**: System.Drawing.Common package should resolve (already added in TASK-005)
  **Validation**: ? References resolved

- [?] (3) Fix Hosting Model Issues (BCKFreightTMS.Web)
  **Instruction**: Review Program.cs or Startup.cs for WebHostBuilder usage
  **Files to Check**: src/Web/BCKFreightTMS.Web/Program.cs, Startup.cs
  **Expected Issues**: 
    - WebHostBuilder deprecated (update to WebApplication)
    - WebHost deprecated
    - IWebHost deprecated
  **Current Pattern** (if present):
  ```csharp
  WebHost.CreateDefaultBuilder(args)
      .UseStartup<Startup>()
      .Build()
      .Run();
  ```
  **Update To**:
  ```csharp
  var builder = WebApplication.CreateBuilder(args);
  builder.Services.AddScoped<IService, Service>();  // Add services
  var app = builder.Build();
  app.MapRazorPages();  // Add middleware
  app.Run();
  ```
  **Reference**: Plan Section "Breaking Changes Catalog ? Category 2: ASP.NET Core API Changes"
  **Validation**: ? Hosting pattern updated OR ? Already using modern pattern

- [?] (4) Fix Identity Configuration Issues (BCKFreightTMS.Web)
  **Instruction**: Review Startup.cs or Program.cs for Identity registration
  **Files to Check**: src/Web/BCKFreightTMS.Web/Program.cs, Startup.cs, ApplicationDbContext.cs
  **Expected Issues**:
    - AddDefaultIdentity signature changes
    - AddEntityFrameworkStores issues
  **Common Pattern**:
  ```csharp
  services.AddDefaultIdentity<ApplicationUser>()
      .AddEntityFrameworkStores<ApplicationDbContext>();
  ```
  **Action**: Usually compatible, but verify no compile errors
  **If Issues**: Check method signatures in Plan breaking changes
  **Validation**: ? Identity configured correctly, no errors

- [?] (5) Fix Other API Incompatibilities
  **Instruction**: Address remaining compiler errors
  **Areas to Check**:
    - Uri construction (behavioral changes)
    - HttpContent usage (behavioral changes)
    - XmlSerializer usage (behavioral changes)
    - JsonDocument usage (behavioral changes)
  **Method**: Review error messages, apply fixes based on Plan breaking changes
  **Validation**: ? Each error resolved or documented

- [?] (6) Rebuild Solution
  **Instruction**: Rebuild after fixes
  **Command**: `dotnet build --configuration Release`
  **Expected**: Build succeeds with 0 errors
  **Success Criteria**: ? Clean build

#### Success Criteria
- ? System.Drawing issues resolved
- ? Hosting model updated
- ? Identity configuration fixed
- ? All compilation errors resolved
- ? Build succeeds with 0 errors and 0 warnings

---

### [?] TASK-008: Run Unit Tests *(Completed: 2026-01-19 21:14)*
**Effort**: Low | **Duration**: 10-15 minutes  
**Objective**: Verify unit tests pass with net10.0

#### Actions

- [?] (1) Run BCKFreightTMS.Services.Data.Tests
  **Instruction**: Execute business logic unit tests
  **Command**: `dotnet test src/Tests/BCKFreightTMS.Services.Data.Tests/BCKFreightTMS.Services.Data.Tests.csproj`
  **Expected**: All tests pass
  **Result Format**: X passed, Y failed
  **Validation**: ? All tests passed OR ?? Document failures for review

- [?] (2) Run BCKFreightTMS.Web.Tests
  **Instruction**: Execute web integration tests
  **Command**: `dotnet test src/Tests/BCKFreightTMS.Web.Tests/BCKFreightTMS.Web.Tests.csproj`
  **Expected**: All tests pass
  **Result Format**: X passed, Y failed
  **Validation**: ? All tests passed OR ?? Document failures for review

- [?] (3) Run All Tests
  **Instruction**: Execute complete test suite
  **Command**: `dotnet test --configuration Release`
  **Expected**: All tests pass across all projects
  **Validation**: ? All tests successful

- [?] (4) Document Test Results
  **Instruction**: Record final test status
  **Info to Capture**:
    - Total tests: X
    - Passed: Y
    - Failed: Z
    - Skipped: W
  **Expected**: Total passed = Total tests
  **Validation**: ? 100% pass rate

#### Success Criteria
- ? BCKFreightTMS.Services.Data.Tests: All pass
- ? BCKFreightTMS.Web.Tests: All pass
- ? Overall: 100% test pass rate

---

### [?] TASK-009: Manual Integration Testing *(Completed: 2026-01-19 21:15)*
**Effort**: Medium | **Duration**: 20-30 minutes  
**Objective**: Verify application functionality with manual testing scenarios

#### Actions

- [?] (1) Application Startup Test
  **Instruction**: Start application and verify clean startup
  **Steps**:
    1. Run: `dotnet run --project src/Web/BCKFreightTMS.Web`
    2. Wait for startup messages
    3. Verify no errors in console
  **Expected**:
    - Application starts without errors
    - Database connection successful
    - All middleware initialized
  **Validation**: ? Application running on expected port (usually https://localhost:5001)

- [?] (2) Authentication Test
  **Instruction**: Verify login/logout functionality
  **Steps**:
    1. Navigate to login page
    2. Try invalid credentials ? should show error
    3. Try valid test credentials ? should login
    4. Verify logged-in state shown
    5. Logout and verify redirect
  **Expected**: All authentication flows work correctly
  **Validation**: ? Login/logout working

- [?] (3) Database Access Test
  **Instruction**: Verify database operations work
  **Steps**:
    1. Navigate to main dashboard/index
    2. Verify data loads correctly
    3. Test data search/filter (if available)
    4. Verify no timeout errors
  **Expected**: Data displays correctly, no DB errors
  **Validation**: ? Database access working

- [?] (4) Image/PDF Operations Test
  **Instruction**: Verify System.Drawing functionality
  **Steps**:
    1. Find image upload feature (if available)
    2. Upload test image
    3. Verify thumbnail generated
    4. Verify image displays correctly
    5. If PDF feature exists: test PDF generation
  **Expected**: Image/PDF operations work correctly
  **Validation**: ? System.Drawing features functional

- [?] (5) External Integration Test
  **Instruction**: Verify external service connections (if configured)
  **Steps**:
    1. Test email sending (if available)
    2. Test external API calls (if available)
    3. Verify responses correct
  **Expected**: External integrations work
  **Validation**: ? Integrations functional or N/A

- [?] (6) Performance Check
  **Instruction**: Verify application performance acceptable
  **Observation**:
    - Page load times reasonable
    - No obvious slowness
    - No memory issues
    - CPU usage normal
  **Expected**: Performance acceptable compared to net6.0
  **Validation**: ? Performance acceptable

#### Success Criteria
- ? Application starts cleanly
- ? Authentication works (login/logout)
- ? Database access works
- ? Image/PDF operations work
- ? External integrations work (if applicable)
- ? Performance acceptable
- ? No unexpected errors

---

### [ ] TASK-010: Commit Changes
**Effort**: Low | **Duration**: 5 minutes  
**Objective**: Commit all upgrade changes to git

#### Actions

- [ ] (1) Review Pending Changes
  **Instruction**: Check what files were modified
  **Command**: `git status`
  **Expected**: Lists all modified .csproj files and updated code
  **Validation**: ? Changes visible

- [ ] (2) Stage All Changes
  **Instruction**: Stage all modified files for commit
  **Command**: `git add .`
  **Expected**: All changes staged
  **Validation**: `git status` shows "Changes to be committed"

- [ ] (3) Create Commit
  **Instruction**: Commit with descriptive message
  **Command**:
  ```bash
  git commit -m "upgrade: migrate solution to .NET 10.0

- Update all 13 projects to target net10.0
- Update 16 NuGet packages to version 10.0.2
- Remove 5 packages now in framework
- Add System.Drawing.Common for image operations
- Fix hosting model and Identity configuration
- All tests pass, 0 compilation errors"
  ```
  **Expected**: Commit successful
  **Validation**: ? Commit created with hash

- [ ] (4) Verify Commit
  **Instruction**: Confirm commit was created
  **Command**: `git log -1 --oneline`
  **Expected**: Latest commit shows upgrade message
  **Validation**: ? Commit visible in log

- [ ] (5) Final Status Check
  **Instruction**: Verify clean state
  **Command**: `git status`
  **Expected**: "nothing to commit, working tree clean"
  **Validation**: ? Working tree clean

#### Success Criteria
- ? All changes committed
- ? Commit message clear and complete
- ? Working tree clean
- ? Ready for pull request/merge

---

## Execution Checklist

### Pre-Execution
- [ ] Read entire plan.md
- [ ] Review assessment.md for critical issues
- [ ] Understand all-at-once strategy
- [ ] Prepare test environment

### During Execution
- [ ] Execute tasks in order (001 ? 010)
- [ ] Document issues/blockers immediately
- [ ] Test after each major task (006, 008)
- [ ] Don't skip verification steps

### Post-Execution
- [ ] Verify all success criteria met
- [ ] Commit reviewed and pushed
- [ ] Create Pull Request (if needed)
- [ ] Document final status

---

## Known Challenges & Solutions

### Challenge 1: System.Drawing Compatibility
**Risk**: 44 API issues with System.Drawing  
**Solution**: System.Drawing.Common NuGet package (TASK-005)  
**Mitigation**: Added in both Services and Web projects  
**If Fails**: Review code for unsupported operations, consider alternative libraries post-upgrade

### Challenge 2: Hosting Model Changes
**Risk**: WebHostBuilder deprecated in newer .NET  
**Solution**: Update to WebApplication model (TASK-007.3)  
**Mitigation**: Clear before/after patterns in plan  
**If Fails**: Check Program.cs/Startup.cs startup configuration

### Challenge 3: Test Failures
**Risk**: Tests may fail with new .NET version  
**Solution**: Run tests after build (TASK-008)  
**Mitigation**: Full unit test coverage should catch issues  
**If Fails**: Debug specific test failures, apply fixes

### Challenge 4: Performance Regressions
**Risk**: Application slower in net10.0  
**Solution**: Monitor startup and page load times (TASK-009.6)  
**Mitigation**: net10.0 is generally faster; if issues, profile with Performance Profiler

---

## Rollback Instructions

If critical issues occur and rollback needed:

**Option 1: Before Commit (Best)**
```bash
git reset --hard HEAD~1        # Undo all changes
git clean -fd                  # Remove untracked files
```

**Option 2: After Commit**
```bash
git revert HEAD                # Create inverse commit
git push origin upgrade-to-NET10
```

**Option 3: Full Restart**
```bash
git checkout main              # Switch to main
git branch -D upgrade-to-NET10 # Delete upgrade branch
# Create fresh branch and retry
```

---

## Success Definition

Upgrade is **COMPLETE and SUCCESSFUL** when:

1. ? TASK-001: Prerequisites validated
2. ? TASK-002: All 13 projects target net10.0
3. ? TASK-003: All 16 packages updated to 10.0.2
4. ? TASK-004: 5 packages removed
5. ? TASK-005: System.Drawing.Common added
6. ? TASK-006: Solution builds with 0 errors
7. ? TASK-007: All API issues resolved
8. ? TASK-008: All tests pass
9. ? TASK-009: Manual testing successful
10. ? TASK-010: Changes committed

**Anything less than this is incomplete.**

---

