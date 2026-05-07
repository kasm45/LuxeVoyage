## Security Audit Notes

### What was checked
- Verified admin/staff authorization boundaries across `Admin*Controller` files.
- Verified admin catalog and user-management mutations are POST-only with anti-forgery validation.
- Verified reservation decision endpoints (`Accept`/`Reject`) remain POST + anti-forgery.
- Verified login `ReturnUrl` handling uses local URL checks before redirecting.
- Reviewed user-deletion protections (current user and last-admin protection).
- Reviewed demo credential exposure in login UI.
- Reviewed destination/public rendering for unsafe raw HTML usage.

### What was fixed
- **Pending-state guard for reservation decisions**
  - `AdminManageBookingsController.Accept` and `Reject` now reject non-pending transitions and return users safely to reservations with a message.
- **Protected archive/system account visibility**
  - `AdminController.Users` now excludes the archive/system user from the Users & Personnel table.
  - `AdminController.DeleteUser` now blocks deletion of the archive/system user explicitly.
- **Development-only demo credentials**
  - `Views/Account/Login.cshtml` now shows seeded demo credentials only when `Env.IsDevelopment()` is true.

### What remains demo-only / production TODO
- Seeded demo credentials are still created by `DbInitializer` for demo convenience; production should disable/demo-gate seed accounts and rotate credentials.
- Cookie hardening defaults should be explicitly reviewed for production (`SecurePolicy`, strict `SameSite` strategy, session settings).
- Add centralized audit logging for privileged role and reservation decision actions.
- Add explicit account lockout/brute-force strategy tuning for production.
- Add formal privacy/terms/legal review before real deployment.

### Assumptions
- Current role policy is intentional: `Admin` for full admin operations, `Personnel` for reservation operations only.
- Existing anti-forgery posture and role annotations in admin mutation flows are considered baseline security controls.
- Current public Razor rendering is HTML-encoded by default and does not intentionally use unsanitized `Html.Raw` for user-submitted values in reviewed files.
