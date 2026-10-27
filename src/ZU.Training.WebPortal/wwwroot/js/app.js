(function () {
  'use strict';
  let apiBase = '', language = 'ar', rememberMe = false, signedIn = false, hints = [], templates = [];
  let currentAccount = null, courses = [], tracks = [], faculties = [], currentCourse = null, instructorChoice = null;
  let adminRoles = [], adminUsers = [], selectedUser = null;
  let adminCourseRows = [];
  let qualityNeeds = [], cancellationRequests = [];
  const labels = {
    ar: {
      brand: 'جامعة الزرقاء', platform: 'منصة التدريب والتطوير المهني', eyebrow: 'منصة الجامعة الرسمية',
      heading: 'تطوير مهني يبدأ بخطوة واضحة', intro: 'ادخل برقمك الوظيفي لمتابعة الدورات والحضور والشهادات من مكان واحد.',
      point1: 'الدورات التدريبية', point2: 'الحضور والتقييم', point3: 'الشهادات الرقمية', loginTitle: 'تسجيل الدخول',
      loginHelp: 'أدخل الرقم الوظيفي وكلمة المرور الخاصة بك.', employeeNumber: 'الرقم الوظيفي', password: 'كلمة المرور',
      remember: 'تذكرني', login: 'دخول', changeTitle: 'تعيين كلمة مرور جديدة',
      changeHelp: 'لأمان حسابك، غيّر كلمة المرور الأولية قبل استخدام المنصة.', oldPassword: 'كلمة المرور الأولية',
      newPassword: 'كلمة المرور الجديدة', confirmPassword: 'تأكيد كلمة المرور', passwordRule: 'اتبع سياسة كلمة المرور الحالية.',
      savePassword: 'حفظ والمتابعة', welcome: 'مرحبًا بك', workspace: 'مساحة العمل', logout: 'تسجيل الخروج', roles: 'الأدوار: ',
      hintsTab: 'تلميحات الحقول', policyTab: 'سياسة كلمة المرور', templatesTab: 'قالب الشهادة',
      hintsTitle: 'تلميحات الحقول', hintsHelp: 'يظهر النص عند لمس الحقل أو التركيز عليه، وتسري التعديلات مباشرة.',
      screenKey: 'مفتاح الشاشة', fieldKey: 'مفتاح الحقل', hintAr: 'التلميح بالعربية', hintEn: 'التلميح بالإنجليزية (اختياري)',
      saveHint: 'حفظ التلميح', clear: 'إفراغ', actions: 'إجراءات', edit: 'تعديل', remove: 'حذف',
      policyTitle: 'سياسة كلمة المرور', policyHelp: 'تطبق القواعد الجديدة عند تغيير كلمة المرور القادم، ولا تغيّر الكلمات الحالية.',
      minimumLength: 'الحد الأدنى للأحرف', maximumLength: 'الحد الأعلى للأحرف', allowSpaces: 'السماح بالمسافات',
      requireMix: 'اشتراط حروف وأرقام', blockCommon: 'منع كلمات المرور الشائعة', savePolicy: 'حفظ السياسة',
      templatesTitle: 'قالب الشهادة المعتمد', templatesHelp: 'ارفع قالب PDF الثابت في أي وقت. يحتفظ النظام بنسخ القوالب المستخدمة سابقًا.',
      templateNameAr: 'اسم القالب بالعربية', templateNameEn: 'الاسم بالإنجليزية (اختياري)', pdfFile: 'ملف PDF',
      uploadTemplate: 'رفع القالب', version: 'النسخة', uploadDate: 'تاريخ الرفع', download: 'تنزيل',
      otherComing: 'وحدات الدورات قيد التطوير؛ لا توجد إجراءات متاحة لهذا الدور بعد.',
      missingLogin: 'أدخل الرقم الوظيفي وكلمة المرور.', invalidLogin: 'بيانات الدخول غير صحيحة أو الحساب غير متاح.',
      passwordMismatch: 'تأكيد كلمة المرور غير مطابق.', changeFailed: 'تعذر تغيير كلمة المرور. تحقق من الكلمة الحالية والسياسة.',
      connectionError: 'تعذر الاتصال بالخادم.', saved: 'تم الحفظ.', deleted: 'تم الحذف.', uploaded: 'تم رفع القالب.',
      failed: 'تعذر تنفيذ العملية.', invalidHint: 'املأ مفتاح الشاشة والحقل والتلميح العربي.',
      invalidPolicy: 'تحقق من الحدود الرقمية لسياسة كلمة المرور.', invalidPdf: 'اختر ملف PDF لا يزيد عن 20 ميغابايت.',
      confirmDelete: 'هل تريد حذف هذا التلميح؟', noRows: 'لا توجد بيانات بعد.',
      coursesTitle: 'الدورات التدريبية', coursesHelp: 'الدورات المتاحة لك ومتابعة حالة المشاركة.', newCourse: 'إنشاء دورة',
      courseFormTitle: 'بيانات الدورة', courseFormHelp: 'الحقول العربية مطلوبة، والإنجليزية اختيارية.',
      courseTitleAr: 'اسم الدورة بالعربية', courseTitleEn: 'اسم الدورة بالإنجليزية (اختياري)', trainingTrack: 'المحور التدريبي',
      deliveryMode: 'نمط الانعقاد', inPerson: 'وجاهي', online: 'عن بعد', hybrid: 'مدمج', objectivesAr: 'الأهداف ونواتج التعلم بالعربية',
      objectivesEn: 'الأهداف بالإنجليزية (اختياري)', audienceAr: 'الفئة المستهدفة بالعربية',
      audienceEn: 'الفئة المستهدفة بالإنجليزية (اختياري)', plannedHours: 'عدد الساعات',
      startDate: 'موعد البداية (YYYY-MM-DD HH:mm)', endDate: 'موعد النهاية (YYYY-MM-DD HH:mm)',
      faculty: 'الكلية (فارغ للدورة العامة)', enrollmentMode: 'طريقة الالتحاق', selfRegistration: 'تسجيل ذاتي',
      qualitySelection: 'اختيار الجودة للمشاركين', actualInstructor: 'اسم المحاضر الفعلي بالعربية',
      instructorSearch: 'ابحث عن حساب المحاضر بالاسم أو الرقم', manualInstructor: 'دون حساب محاضر',
      qualityOperated: 'تدير الجودة الدورة القصيرة', hasExam: 'الدورة مشروطة بامتحان',
      evaluationDeadline: 'نهاية فترة التقييم (للجودة)', saveCourse: 'حفظ الدورة', cancel: 'إلغاء', close: 'إغلاق',
      view: 'عرض', enroll: 'التسجيل', registered: 'مسجل', approved: 'معتمدة', submitted: 'بانتظار الجودة',
      inProgress: 'جارية', ended: 'منتهية', cancelled: 'ملغاة', noCourses: 'لا توجد دورات ظاهرة لحسابك.',
      invalidCourse: 'أكمل الحقول العربية والمواعيد، واختر محاضرًا من القائمة أو تشغيل الجودة.',
      courseCreated: 'تم حفظ الدورة.', enrolled: 'تم تسجيلك.', lectureTitle: 'عنوان المحاضرة بالعربية',
      lectureDate: 'موعد المحاضرة (اختياري)', addLecture: 'إضافة محاضرة', lectures: 'المحاضرات',
      activateCode: 'تفعيل الكود 5 دقائق', attendanceCode: 'كود الحضور', submitCode: 'تأكيد الحضور',
      attendance: 'الحضور', participants: 'المشاركون', addParticipant: 'إضافة مشارك', searchParticipant: 'ابحث عن مشارك',
      endCourse: 'إنهاء الدورة', confirmEnd: 'هل تريد إنهاء الدورة؟ لا يمكن إضافة محاضرات بعدها.',
      exam: 'الامتحان', examName: 'اسم الامتحان بالعربية', createExam: 'إنشاء الامتحان',
      notRecorded: 'لم تُرصد النتيجة', passed: 'ناجح', failed: 'راسب', approveCourse: 'اعتماد الدورة',
      selectPerson: 'اختر شخصًا من النتائج', codeActive: 'الكود مفعل حتى', codeAccepted: 'تم تسجيل الحضور.',
      noLectures: 'لم تُنشأ محاضرات بعد.', notEligibleExternal: 'مقترح المحاضر الخارجي يحتاج مسار الكتاب الرسمي والسيرة الذاتية.',
      usersTab: 'المستخدمون', rolePermissionsTab: 'صلاحيات الأدوار', usersTitle: 'إدارة المستخدمين',
      usersHelp: 'ابحث بالاسم أو رقم الدخول، ثم افتح التفاصيل والصلاحيات.', userSearch: 'الاسم أو الرقم الوظيفي',
      search: 'بحث', newUser: 'إنشاء حساب', accountType: 'نوع الحساب', employee: 'موظف', externalTrainer: 'محاضر خارجي',
      externalLogin: 'رقم المحاضر الخارجي (4 أرقام)', nameAr: 'الاسم بالعربية', nameEn: 'الاسم بالإنجليزية (اختياري)',
      email: 'البريد الإلكتروني', externalPassword: 'كلمة مرور المحاضر الخارجي', rolesLabel: 'الأدوار',
      saveUser: 'حفظ الحساب', showPasswords: 'عرض كلمات المرور', disableUser: 'تعطيل الحساب',
      restoreUser: 'استعادة الحساب', allowAccess: 'تجاوز حالة العمل', removeAccessOverride: 'إزالة تجاوز العمل',
      changePassword: 'تغيير كلمة المرور', saveRoles: 'حفظ الأدوار', userPermissions: 'استثناءات صلاحيات المستخدم',
      userPermissionsHelp: 'لكل صلاحية: اتباع الدور أو سماح أو منع، مع النتيجة الفعلية.',
      sharedPassword: 'كلمة المرور الأولية المشتركة', sharedHelp: 'تعمل فقط للموظف الذي لم يكمل أول دخول.',
      showShared: 'عرض الكلمة الحالية', newShared: 'كلمة جديدة', saveShared: 'حفظ الكلمة المشتركة',
      rolePermissionsTitle: 'صلاحيات الأدوار', rolePermissionsHelp: 'تغيير الصلاحية الافتراضية ينعكس على جميع مستخدمي الدور ما لم يكن لديهم استثناء فردي.',
      inherited: 'اتباع الدور', allow: 'سماح', deny: 'منع', effective: 'النتيجة', yes: 'مسموح', no: 'ممنوع',
      initialPassword: 'الأولية', currentPassword: 'الحالية', confirmDisable: 'هل تريد تعطيل هذا الحساب؟',
      userCreated: 'تم إنشاء الحساب.', invalidUser: 'تحقق من بيانات الحساب والأدوار.', noUsers: 'لا يوجد مستخدمون.',
      universityTab: 'بيانات الجامعة', universityTitle: 'مزامنة بيانات الجامعة',
      universityHelp: 'يجلب الزر الكليات والأقسام والموظفين إلى قاعدة المنصة، ويحافظ على كلمات المرور والأدوار المحلية.',
      refreshUniversity: 'تحديث بيانات الجامعة', syncRuns: 'آخر عمليات التحديث',
      missingEmployees: 'موظفون اختفوا من آخر استجابة مكتملة', syncComplete: 'اكتمل التحديث',
      syncIncomplete: 'لم يكتمل التحديث', added: 'مضاف', updated: 'محدّث', missing: 'مفقود',
      needsTab: 'الاحتياجات التدريبية', cancellationsTab: 'طلبات الإلغاء', needsTitle: 'الاحتياجات التدريبية',
      needsHelp: 'يرفع العميد الاحتياج وكتابه الرسمي إلى الجودة فقط.', needTitleAr: 'عنوان الاحتياج بالعربية',
      needTitleEn: 'العنوان بالإنجليزية (اختياري)', needDescriptionAr: 'وصف الاحتياج بالعربية',
      needDescriptionEn: 'الوصف بالإنجليزية (اختياري)', officialLetter: 'الكتاب الرسمي PDF أو DOCX',
      submitNeed: 'رفع الاحتياج للجودة', cancellationsTitle: 'طلبات إلغاء الدورات',
      cancellationsHelp: 'لا تُلغى الدورة المعتمدة إلا بعد كتاب رسمي وقرار الجودة.',
      returnRevision: 'إعادة للتعديل', reject: 'رفض', resubmit: 'إعادة إرسال',
      requestCancellation: 'طلب إلغاء الدورة', cancellationReason: 'سبب الإلغاء بالعربية',
      reviewNote: 'ملاحظة الجودة أو سبب القرار', officialDocument: 'الكتاب الرسمي',
      revisionRequested: 'أعيد للتعديل', rejected: 'مرفوض', needsEmpty: 'لا توجد طلبات بعد.',
      evaluationTab: 'نموذج التقييم', evaluationAdminHelp: 'نشر نسخة جديدة يطلب إعادة التقييم في الدورات التي ما زالت فترة تقييمها مفتوحة.',
      addQuestion: 'إضافة سؤال', addOption: 'إضافة خيار', publishEvaluation: 'نشر نسخة جديدة',
      questionAr: 'السؤال بالعربية', questionEn: 'السؤال بالإنجليزية (اختياري)', optionAr: 'الخيار بالعربية',
      optionEn: 'الخيار بالإنجليزية (اختياري)', evaluateCourse: 'تقييم الدورة', submitEvaluation: 'إرسال التقييم',
      evaluationSubmitted: 'تم إرسال تقييم هذه النسخة.', impactTitle: 'الأثر التدريبي',
      impactFile: 'ملف الأثر PDF أو DOCX', uploadImpact: 'رفع الأثر', impactPending: 'بانتظار مراجعة المحاضر',
      impactReturned: 'أعيد للتعديل', impactApproved: 'معتمد', approveImpact: 'اعتماد الأثر',
      returnImpact: 'إعادة للتعديل', evaluationStats: 'إحصاءات التقييم', extendDeadline: 'تمديد فترة التقييم',
      examRequirement: 'شرط الامتحان', examRequired: 'بامتحان', examNotRequired: 'بلا امتحان', changeExamRequirement: 'حفظ شرط الامتحان',
      certificateGate: 'متطلبات الشهادة', eligible: 'استوفيت المتطلبات', notEligible: 'لم تستوفِ المتطلبات بعد',
      gateCourseNotEnded: 'الدورة لم تنتهِ', gateNoLectures: 'لا توجد محاضرات', gateAttendance: 'الحضور أقل من 75%',
      gateExam: 'لم يثبت نجاح الامتحان', gateEvaluation: 'التقييم غير مكتمل', gateImpact: 'الأثر غير معتمد',
      verifyCertificate: 'التحقق من شهادة', certificateReference: 'الرقم المرجعي للشهادة', verify: 'تحقق',
      certificateValid: 'شهادة صالحة ومعتمدة', certificateRevoked: 'الشهادة ملغاة', certificateNotFound: 'لم يُعثر على شهادة بهذا الرقم',
      recipient: 'صاحب الشهادة', courseName: 'الدورة', instructor: 'المحاضر', issuedAt: 'تاريخ الإصدار',
      reviewProposal: 'قرار الجودة بشأن المقترح', resubmitProposal: 'تعديل المقترح وإعادة إرساله',
      proposalReviewNote: 'سبب قرار الجودة بالعربية', proposalUpdated: 'أُعيد إرسال المقترح',
      adminCoursesTab: 'إدارة الدورات', adminCoursesHelp: 'تعطيل الدورة منطقيًا أو استعادتها، مع قرار ظهور شهاداتها.',
      disableCourse: 'تعطيل الدورة', restoreCourse: 'استعادة الدورة', hideCertificates: 'إخفاء الشهادات من التحقق',
      keepCertificates: 'إبقاء الشهادات', courseDeleteWarning: 'هل تؤكد تعطيل الدورة؟ ستختفي من قوائم الدورات النشطة.',
      courseActive: 'نشطة', courseDisabled: 'معطلة', certificates: 'الشهادات'
    },
    en: {
      brand: 'Zarqa University', platform: 'Professional Training and Development Platform', eyebrow: 'Official university platform',
      heading: 'Professional growth starts here', intro: 'Sign in with your employee number to manage courses, attendance, and certificates.',
      point1: 'Training courses', point2: 'Attendance and evaluation', point3: 'Digital certificates', loginTitle: 'Sign in',
      loginHelp: 'Enter your employee number and password.', employeeNumber: 'Employee number', password: 'Password',
      remember: 'Remember me', login: 'Sign in', changeTitle: 'Set a new password',
      changeHelp: 'Change your initial password before using the platform.', oldPassword: 'Initial password',
      newPassword: 'New password', confirmPassword: 'Confirm password', passwordRule: 'Follow the current password policy.',
      savePassword: 'Save and continue', welcome: 'Welcome', workspace: 'Workspace', logout: 'Sign out', roles: 'Roles: ',
      hintsTab: 'Field hints', policyTab: 'Password policy', templatesTab: 'Certificate template',
      hintsTitle: 'Field hints', hintsHelp: 'Hints appear when a field is focused. Changes take effect immediately.',
      screenKey: 'Screen key', fieldKey: 'Field key', hintAr: 'Arabic hint', hintEn: 'English hint (optional)',
      saveHint: 'Save hint', clear: 'Clear', actions: 'Actions', edit: 'Edit', remove: 'Delete',
      policyTitle: 'Password policy', policyHelp: 'New rules apply when passwords are next changed. Existing passwords remain valid.',
      minimumLength: 'Minimum length', maximumLength: 'Maximum length', allowSpaces: 'Allow spaces',
      requireMix: 'Require letters and digits', blockCommon: 'Block common passwords', savePolicy: 'Save policy',
      templatesTitle: 'Approved certificate template', templatesHelp: 'Upload a fixed PDF at any time. Older versions are retained.',
      templateNameAr: 'Arabic template name', templateNameEn: 'English name (optional)', pdfFile: 'PDF file',
      uploadTemplate: 'Upload template', version: 'Version', uploadDate: 'Upload date', download: 'Download',
      otherComing: 'Course modules are in development; no actions are available for this role yet.',
      missingLogin: 'Enter your employee number and password.', invalidLogin: 'Invalid credentials or unavailable account.',
      passwordMismatch: 'Passwords do not match.', changeFailed: 'Could not change the password. Check the current password and policy.',
      connectionError: 'Cannot connect to the server.', saved: 'Saved.', deleted: 'Deleted.', uploaded: 'Template uploaded.',
      failed: 'Operation failed.', invalidHint: 'Enter the screen key, field key, and Arabic hint.',
      invalidPolicy: 'Check the password length limits.', invalidPdf: 'Choose a PDF file no larger than 20 MB.',
      confirmDelete: 'Delete this hint?', noRows: 'No data yet.',
      coursesTitle: 'Training courses', coursesHelp: 'Courses available to you and your participation status.', newCourse: 'Create course',
      courseFormTitle: 'Course details', courseFormHelp: 'Arabic fields are required; English fields are optional.',
      courseTitleAr: 'Arabic course title', courseTitleEn: 'English course title (optional)', trainingTrack: 'Training track',
      deliveryMode: 'Delivery mode', inPerson: 'In person', online: 'Online', hybrid: 'Hybrid', objectivesAr: 'Arabic objectives and outcomes',
      objectivesEn: 'English objectives (optional)', audienceAr: 'Arabic target audience', audienceEn: 'English audience (optional)',
      plannedHours: 'Planned hours', startDate: 'Start (YYYY-MM-DD HH:mm)', endDate: 'End (YYYY-MM-DD HH:mm)',
      faculty: 'Faculty (blank for general)', enrollmentMode: 'Enrollment mode', selfRegistration: 'Self registration',
      qualitySelection: 'Quality selects participants', actualInstructor: 'Actual instructor name in Arabic',
      instructorSearch: 'Search instructor by name or number', manualInstructor: 'No instructor account',
      qualityOperated: 'Quality operates short course', hasExam: 'Exam is required', evaluationDeadline: 'Evaluation deadline (Quality)',
      saveCourse: 'Save course', cancel: 'Cancel', close: 'Close', view: 'View', enroll: 'Enroll', registered: 'Enrolled',
      approved: 'Approved', submitted: 'Awaiting Quality', inProgress: 'In progress', ended: 'Ended', cancelled: 'Cancelled',
      noCourses: 'No courses visible to your account.', invalidCourse: 'Complete the Arabic fields and dates, and select an instructor or Quality operation.',
      courseCreated: 'Course saved.', enrolled: 'You are enrolled.', lectureTitle: 'Arabic lecture title',
      lectureDate: 'Lecture time (optional)', addLecture: 'Add lecture', lectures: 'Lectures',
      activateCode: 'Activate code for 5 minutes', attendanceCode: 'Attendance code', submitCode: 'Record attendance',
      attendance: 'Attendance', participants: 'Participants', addParticipant: 'Add participant', searchParticipant: 'Search participant',
      endCourse: 'End course', confirmEnd: 'End this course? No more lectures can be added.', exam: 'Exam',
      examName: 'Arabic exam name', createExam: 'Create exam', notRecorded: 'Not recorded', passed: 'Passed', failed: 'Failed',
      approveCourse: 'Approve course', selectPerson: 'Select a person from results', codeActive: 'Code active until',
      codeAccepted: 'Attendance recorded.', noLectures: 'No lectures yet.',
      notEligibleExternal: 'External instructor proposals require the official letter and CV workflow.',
      usersTab: 'Users', rolePermissionsTab: 'Role permissions', usersTitle: 'User management',
      usersHelp: 'Search by name or login number, then open account details and permissions.', userSearch: 'Name or employee number',
      search: 'Search', newUser: 'Create account', accountType: 'Account type', employee: 'Employee', externalTrainer: 'External instructor',
      externalLogin: 'External instructor number (4 digits)', nameAr: 'Arabic name', nameEn: 'English name (optional)',
      email: 'Email', externalPassword: 'External instructor password', rolesLabel: 'Roles', saveUser: 'Save account',
      showPasswords: 'Show passwords', disableUser: 'Disable account', restoreUser: 'Restore account',
      allowAccess: 'Override work status', removeAccessOverride: 'Remove work override', changePassword: 'Change password',
      saveRoles: 'Save roles', userPermissions: 'User permission overrides',
      userPermissionsHelp: 'Use role default, allow, or deny for each permission. The effective result is shown.',
      sharedPassword: 'Shared initial password', sharedHelp: 'Works only until an employee completes first login.',
      showShared: 'Show current password', newShared: 'New password', saveShared: 'Save shared password',
      rolePermissionsTitle: 'Role permissions', rolePermissionsHelp: 'Default role changes affect all members unless they have a personal override.',
      inherited: 'Use role', allow: 'Allow', deny: 'Deny', effective: 'Effective', yes: 'Allowed', no: 'Denied',
      initialPassword: 'Initial', currentPassword: 'Current', confirmDisable: 'Disable this account?',
      userCreated: 'Account created.', invalidUser: 'Check account details and roles.', noUsers: 'No users found.',
      universityTab: 'University data', universityTitle: 'University directory sync',
      universityHelp: 'Refreshes faculties, departments, and employees while preserving local passwords and roles.',
      refreshUniversity: 'Refresh university data', syncRuns: 'Recent sync runs',
      missingEmployees: 'Employees missing from the last complete response', syncComplete: 'Sync complete',
      syncIncomplete: 'Sync incomplete', added: 'Added', updated: 'Updated', missing: 'Missing',
      needsTab: 'Training needs', cancellationsTab: 'Cancellation requests', needsTitle: 'Training needs',
      needsHelp: 'The dean sends needs and an official letter to Quality only.', needTitleAr: 'Arabic need title',
      needTitleEn: 'English title (optional)', needDescriptionAr: 'Arabic description',
      needDescriptionEn: 'English description (optional)', officialLetter: 'Official PDF or DOCX letter',
      submitNeed: 'Send need to Quality', cancellationsTitle: 'Course cancellation requests',
      cancellationsHelp: 'An approved course needs an official letter and Quality decision to cancel.',
      returnRevision: 'Return for revision', reject: 'Reject', resubmit: 'Resubmit',
      requestCancellation: 'Request cancellation', cancellationReason: 'Arabic cancellation reason',
      reviewNote: 'Quality note or decision reason', officialDocument: 'Official letter',
      revisionRequested: 'Returned for revision', rejected: 'Rejected', needsEmpty: 'No requests yet.',
      evaluationTab: 'Evaluation form', evaluationAdminHelp: 'Publishing a new version requires reevaluation in courses with an open evaluation period.',
      addQuestion: 'Add question', addOption: 'Add option', publishEvaluation: 'Publish new version',
      questionAr: 'Arabic question', questionEn: 'English question (optional)', optionAr: 'Arabic option',
      optionEn: 'English option (optional)', evaluateCourse: 'Evaluate course', submitEvaluation: 'Submit evaluation',
      evaluationSubmitted: 'This version was submitted.', impactTitle: 'Training impact',
      impactFile: 'Impact PDF or DOCX', uploadImpact: 'Upload impact', impactPending: 'Awaiting instructor review',
      impactReturned: 'Returned for revision', impactApproved: 'Approved', approveImpact: 'Approve impact',
      returnImpact: 'Return for revision', evaluationStats: 'Evaluation statistics', extendDeadline: 'Extend evaluation period',
      examRequirement: 'Exam requirement', examRequired: 'Exam required', examNotRequired: 'No exam', changeExamRequirement: 'Save exam requirement',
      certificateGate: 'Certificate requirements', eligible: 'Requirements met', notEligible: 'Requirements not yet met',
      gateCourseNotEnded: 'Course has not ended', gateNoLectures: 'No lectures', gateAttendance: 'Attendance below 75%',
      gateExam: 'Exam pass not recorded', gateEvaluation: 'Evaluation incomplete', gateImpact: 'Impact not approved',
      verifyCertificate: 'Verify certificate', certificateReference: 'Certificate reference', verify: 'Verify',
      certificateValid: 'Valid official certificate', certificateRevoked: 'Certificate revoked', certificateNotFound: 'No certificate found for this reference',
      recipient: 'Recipient', courseName: 'Course', instructor: 'Instructor', issuedAt: 'Issued on',
      reviewProposal: 'Quality decision on proposal', resubmitProposal: 'Edit and resubmit proposal',
      proposalReviewNote: 'Arabic reason for the decision', proposalUpdated: 'Proposal resubmitted',
      adminCoursesTab: 'Manage courses', adminCoursesHelp: 'Logically disable or restore a course and choose certificate visibility.',
      disableCourse: 'Disable course', restoreCourse: 'Restore course', hideCertificates: 'Hide certificates from verification',
      keepCertificates: 'Keep certificates', courseDeleteWarning: 'Disable this course? It will disappear from active course lists.',
      courseActive: 'Active', courseDisabled: 'Disabled', certificates: 'Certificates'
    }
  };
  const t = key => labels[language][key] || key;
  function api(path, method, data) {
    return $.ajax({ url: apiBase + path, method: method, cache: false, contentType: 'application/json; charset=utf-8',
      data: data ? JSON.stringify(data) : undefined, xhrFields: { withCredentials: true }, timeout: 15000 });
  }
  function notice(selector, value, success) {
    $(selector).removeClass('d-none alert-danger alert-success').addClass(success ? 'alert-success' : 'alert-danger').text(value);
  }
  function show(view) {
    $('#login-view, #change-view, #verification-view, #dashboard-view').addClass('d-none'); $('#' + view + '-view').removeClass('d-none');
    const dashboard = view === 'dashboard'; $('.page-shell').toggleClass('dashboard-mode', dashboard);
    $('.intro, .auth-card').toggleClass('d-none', dashboard); $('#status-message').addClass('d-none').text('');
  }
  function translate() {
    document.documentElement.lang = language; document.documentElement.dir = language === 'ar' ? 'rtl' : 'ltr';
    $('#bootstrap-style').attr('href', language === 'ar' ? '/vendor/bootstrap.rtl.min.css' : '/vendor/bootstrap.min.css');
    $('[data-i18n]').each(function () { $(this).text(t($(this).data('i18n'))); });
    $('[data-i18n-placeholder]').each(function () { $(this).attr('placeholder', t($(this).data('i18n-placeholder'))); });
    $('#language-toggle').text(language === 'ar' ? 'English' : 'العربية');
    renderHints(); renderTemplates(); renderCourseList(); renderCourseSelects();
    renderAdminUsers(); renderRoleSelect(); renderAdminFacultySelect(); renderQualityNeeds(); renderCancellations();
    renderAdminCourses();
    if (currentAccount) $('#account-name').text(name(currentAccount.nameAr, currentAccount.nameEn));
    if (currentCourse) renderCourseDetails();
  }
  function showAccount(account) {
    currentAccount = account;
    signedIn = true;
    if (account.preferredLanguage === 'ar' || account.preferredLanguage === 'en') {
      language = account.preferredLanguage; translate();
    }
    if (account.mustChangePassword) { show('change'); return; }
    $('#account-name').text(language === 'en' && account.nameEn ? account.nameEn : account.nameAr || '');
    $('#account-role').text(t('roles') + (account.roles || []).join('، '));
    const admin = ['admin.hints.manage', 'admin.password_policy.manage', 'admin.certificate_templates.manage',
      'admin.users.manage', 'admin.permissions.manage', 'admin.university_sync.run',
      'evaluation.manage', 'admin.courses.manage'].some(can);
    $('#admin-workspace').toggleClass('d-none', !admin);
    $('[data-admin-tab=hints]').toggleClass('d-none', !can('admin.hints.manage'));
    $('[data-admin-tab=password]').toggleClass('d-none', !can('admin.password_policy.manage'));
    $('[data-admin-tab=templates]').toggleClass('d-none', !can('admin.certificate_templates.manage'));
    $('[data-admin-tab=users]').toggleClass('d-none', !can('admin.users.manage'));
    $('[data-admin-tab=role-permissions]').toggleClass('d-none', !can('admin.permissions.manage'));
    $('[data-admin-tab=university]').toggleClass('d-none', !can('admin.university_sync.run'));
    $('[data-admin-tab=evaluation]').toggleClass('d-none', !can('evaluation.manage'));
    $('[data-admin-tab=admin-courses]').toggleClass('d-none', !can('admin.courses.manage'));
    $('[data-admin-tab]').removeClass('active');
    const firstTab = $('[data-admin-tab]:not(.d-none)').first(); firstTab.addClass('active');
    $('#admin-workspace .workspace-panel').addClass('d-none');
    if (firstTab.length) $('#' + firstTab.data('admin-tab') + '-panel').removeClass('d-none');
    $('#open-course-form').toggleClass('d-none', !can('courses.create'));
    const quality = can('training_needs.view') || can('course_cancellations.view');
    $('#quality-workspace').toggleClass('d-none', !quality);
    $('#need-form').toggleClass('d-none', !can('training_needs.submit'));
    $('[data-quality-tab=needs]').toggleClass('d-none', !can('training_needs.view'));
    $('[data-quality-tab=cancellations]').toggleClass('d-none', !can('course_cancellations.view'));
    $('[data-quality-tab]').removeClass('active');
    const firstQuality = $('[data-quality-tab]:not(.d-none)').first(); firstQuality.addClass('active');
    $('#needs-panel, #cancellations-panel').addClass('d-none');
    if (firstQuality.length) $('#' + firstQuality.data('quality-tab') + '-panel').removeClass('d-none');
    show('dashboard'); if (admin) loadAdmin(); if (can('courses.view')) loadCourses(); if (quality) loadQualityRequests();
    if (can('evaluation.manage')) loadEvaluationEditor();
    if (can('admin.courses.manage')) loadAdminCourses();
  }
  function loadAccount() { api('/api/auth/me', 'GET').done(showAccount).fail(() => { signedIn = false; show('login'); }); }
  function loadAdmin() {
    if (can('admin.hints.manage')) api('/api/settings/hints', 'GET').done(rows => { hints = rows; renderHints(); }).fail(() => notice('#admin-message', t('connectionError')));
    if (can('admin.password_policy.manage')) api('/api/settings/password-policy', 'GET').done(p => {
      $('#policy-min').val(p.minimumLength); $('#policy-max').val(p.maximumLength);
      $('#policy-spaces').prop('checked', p.allowSpaces); $('#policy-mix').prop('checked', p.requireMixedCharacterClasses);
      $('#policy-common').prop('checked', p.blockCommonPasswords);
    });
    if (can('admin.certificate_templates.manage')) api('/api/settings/certificate-templates', 'GET').done(rows => { templates = rows; renderTemplates(); });
    if (can('admin.users.manage')) {
      api('/api/admin/roles', 'GET').done(rows => { adminRoles = rows; renderRoleSelect(); renderRoleChecks('#create-role-list', []); });
      searchAdminUsers();
    }
    if (can('admin.university_sync.run')) loadSyncData();
  }
  function renderHints() {
    const body = $('#hints-body').empty();
    if (!hints.length) { $('<tr>').append($('<td colspan="5">').text(t('noRows'))).appendTo(body); return; }
    hints.forEach(h => {
      const row = $('<tr>');
      $('<td>').text(h.screenKey).appendTo(row); $('<td>').text(h.fieldKey).appendTo(row);
      $('<td>').text(h.textAr).appendTo(row); $('<td>').text(h.textEn || '—').appendTo(row);
      const actions = $('<td>').addClass('text-nowrap').appendTo(row);
      $('<button type="button" class="btn btn-sm btn-outline-primary me-2 edit-hint">').text(t('edit')).data('id', h.id).appendTo(actions);
      $('<button type="button" class="btn btn-sm btn-outline-danger delete-hint">').text(t('remove')).data('id', h.id).appendTo(actions);
      body.append(row);
    });
  }
  function renderTemplates() {
    const body = $('#templates-body').empty();
    if (!templates.length) { $('<tr>').append($('<td colspan="4">').text(t('noRows'))).appendTo(body); return; }
    templates.forEach(item => {
      const row = $('<tr>'); $('<td>').text(item.version).appendTo(row);
      $('<td>').text(language === 'en' && item.nameEn ? item.nameEn : item.nameAr).appendTo(row);
      $('<td>').text(new Date(item.createdAtUtc).toLocaleDateString(language === 'ar' ? 'ar-JO' : 'en-GB')).appendTo(row);
      $('<td>').append($('<a class="btn btn-sm btn-outline-primary">').attr('href', apiBase + '/api/settings/certificate-templates/' + item.id + '/file').text(t('download'))).appendTo(row);
      body.append(row);
    });
  }
  function searchAdminUsers() {
    api('/api/admin/users?query=' + encodeURIComponent($('#user-search').val().trim()) + '&page=1', 'GET')
      .done(rows => { adminUsers = rows; renderAdminUsers(); });
  }
  function renderAdminUsers() {
    const container = $('#user-list').empty();
    if (!adminUsers.length) { $('<p class="muted">').text(t('noUsers')).appendTo(container); return; }
    const table = $('<table class="table table-hover align-middle"><thead><tr><th></th><th></th><th></th><th></th></tr></thead><tbody></tbody></table>').appendTo(container);
    table.find('th').eq(0).text(t('employeeNumber')); table.find('th').eq(1).text(t('nameAr'));
    table.find('th').eq(2).text(t('rolesLabel')); table.find('th').eq(3).text(t('actions'));
    adminUsers.forEach(user => {
      const row = $('<tr>').appendTo(table.find('tbody'));
      $('<td>').text(user.loginName).appendTo(row);
      $('<td>').text(name(user.nameAr, user.nameEn) + (user.isActive ? '' : ' · ' + t('disableUser'))).appendTo(row);
      $('<td>').text(user.roles.join(', ')).appendTo(row);
      $('<td>').append($('<button type="button" class="btn btn-sm btn-outline-primary open-user">').data('id', user.id).text(t('view'))).appendTo(row);
    });
  }
  function renderRoleChecks(selector, selectedKeys) {
    const container = $(selector).empty();
    adminRoles.forEach(role => {
      const item = $('<label class="form-check form-check-inline">').appendTo(container);
      $('<input type="checkbox" class="form-check-input">').val(role.key).prop('checked', selectedKeys.includes(role.key)).appendTo(item);
      $('<span class="form-check-label">').text(name(role.nameAr, role.nameEn)).appendTo(item);
    });
  }
  function renderRoleSelect() {
    const select = $('#role-select'), value = select.val(); select.empty();
    adminRoles.forEach(role => $('<option>').val(role.key).text(name(role.nameAr, role.nameEn)).appendTo(select));
    if (value) select.val(value);
    if (select.val()) loadRoleMatrix();
  }
  function openUser(id) {
    selectedUser = adminUsers.find(x => x.id === id) || null; if (!selectedUser) return;
    $('#user-form-panel').addClass('d-none'); $('#user-detail-panel').removeClass('d-none');
    $('#credential-result').addClass('d-none').text('');
    $('#user-detail-name').text(name(selectedUser.nameAr, selectedUser.nameEn));
    $('#user-detail-meta').text(selectedUser.loginName + ' · ' + (selectedUser.email || '—'));
    $('#toggle-user-active').text(t(selectedUser.isActive ? 'disableUser' : 'restoreUser'));
    $('#toggle-user-access').text(t(selectedUser.accessOverrideAllow ? 'removeAccessOverride' : 'allowAccess'));
    renderRoleChecks('#edit-role-list', selectedUser.roles);
    loadUserMatrix();
    $('#user-detail-panel')[0].scrollIntoView({ behavior: 'smooth', block: 'start' });
  }
  function loadUserMatrix() {
    if (!selectedUser) return;
    api('/api/admin/users/' + selectedUser.id + '/permissions', 'GET').done(rows => {
      const container = $('#user-permission-list').empty();
      const table = $('<table class="table table-sm align-middle"><thead><tr><th></th><th></th><th></th></tr></thead><tbody></tbody></table>').appendTo(container);
      table.find('th').eq(0).text(t('userPermissions')); table.find('th').eq(1).text(t('inherited'));
      table.find('th').eq(2).text(t('effective'));
      rows.forEach(item => {
        const row = $('<tr>').appendTo(table.find('tbody'));
        $('<td>').text(name(item.nameAr, item.nameEn) + ' · ' + item.key).appendTo(row);
        const select = $('<select class="form-select form-select-sm user-override">').data('key', item.key).appendTo($('<td>').appendTo(row));
        [[0, 'inherited'], [1, 'allow'], [2, 'deny']].forEach(([mode, label]) => $('<option>').val(mode).text(t(label)).appendTo(select));
        select.val(item.overrideMode);
        $('<td>').text(t(item.effective ? 'yes' : 'no')).appendTo(row);
      });
    });
  }
  function loadRoleMatrix() {
    const key = $('#role-select').val(); if (!key) return;
    api('/api/admin/roles/' + encodeURIComponent(key) + '/permissions', 'GET').done(rows => {
      const container = $('#role-permission-list').empty();
      const table = $('<table class="table table-sm align-middle"><thead><tr><th></th><th></th></tr></thead><tbody></tbody></table>').appendTo(container);
      table.find('th').eq(0).text(t('userPermissions')); table.find('th').eq(1).text(t('allow'));
      rows.forEach(item => {
        const row = $('<tr>').appendTo(table.find('tbody'));
        $('<td>').text(name(item.nameAr, item.nameEn) + ' · ' + item.key).appendTo(row);
        $('<td>').append($('<input type="checkbox" class="form-check-input role-grant">').prop('checked', item.granted)
          .prop('disabled', key === 'ADMIN').data('key', item.key)).appendTo(row);
      });
    });
  }
  function renderAdminFacultySelect() {
    const select = $('#user-faculty').empty();
    $('<option>').val('').text('—').appendTo(select);
    faculties.forEach(x => $('<option>').val(x.id).text(name(x.nameAr, x.nameEn)).appendTo(select));
  }
  function loadSyncData() {
    api('/api/admin/university-sync/runs', 'GET').done(rows => {
      const box = $('#sync-runs').empty();
      if (!rows.length) { $('<p class="muted">').text(t('noRows')).appendTo(box); return; }
      const table = $('<table class="table table-sm"><tbody></tbody></table>').appendTo(box);
      rows.forEach(x => {
        const row = $('<tr>').appendTo(table.find('tbody'));
        $('<td>').text(x.runId).appendTo(row);
        $('<td>').text(t(x.complete ? 'syncComplete' : 'syncIncomplete')).appendTo(row);
        $('<td>').text(t('added') + ': ' + x.added + ' · ' + t('updated') + ': ' + x.updated + ' · ' + t('missing') + ': ' + x.missing).appendTo(row);
        $('<td>').text(x.error || '').appendTo(row);
      });
    });
    api('/api/admin/university-sync/missing-employees', 'GET').done(rows => {
      const box = $('#missing-employees').empty();
      if (!rows.length) { $('<p class="muted">').text(t('noRows')).appendTo(box); return; }
      const table = $('<table class="table table-sm"><tbody></tbody></table>').appendTo(box);
      rows.forEach(x => {
        const row = $('<tr>').appendTo(table.find('tbody'));
        $('<td>').text(x.employeeId).appendTo(row); $('<td>').text(x.nameAr).appendTo(row);
        $('<td>').text(new Date(x.missingSinceUtc).toLocaleDateString(language === 'ar' ? 'ar-JO' : 'en-GB')).appendTo(row);
      });
    });
  }
  function renderNeedFacultySelect() {
    const select = $('#need-faculty'), value = select.val(); select.empty();
    $('<option>').val('').text('—').appendTo(select);
    faculties.forEach(x => $('<option>').val(x.id).text(name(x.nameAr, x.nameEn)).appendTo(select));
    if (role('DEAN') && !role('ADMIN')) select.val(currentAccount?.facultyId || '').prop('disabled', true);
    else if (value) select.val(value);
  }
  function loadQualityRequests() {
    if (can('training_needs.view')) api('/api/training-needs', 'GET')
      .done(rows => { qualityNeeds = rows; renderQualityNeeds(); });
    if (can('course_cancellations.view')) api('/api/course-cancellations', 'GET')
      .done(rows => { cancellationRequests = rows; renderCancellations(); });
  }
  function requestStatus(status) { return t(({ 0: 'submitted', 1: 'revisionRequested', 2: 'approved', 3: 'rejected' })[status]); }
  function documentLinks(container, ids) {
    ids.forEach((id, index) => $('<a class="btn btn-sm btn-outline-primary me-2 mb-2" target="_blank" rel="noopener">')
      .attr('href', apiBase + '/api/official-documents/' + id)
      .text(t('officialDocument') + ' ' + (index + 1)).appendTo(container));
  }
  function renderQualityNeeds() {
    const box = $('#needs-list').empty();
    if (!qualityNeeds.length) { $('<p class="muted">').text(t('needsEmpty')).appendTo(box); return; }
    qualityNeeds.forEach(need => {
      const card = $('<article class="request-card">').appendTo(box);
      const head = $('<div class="d-flex justify-content-between gap-2 flex-wrap">').appendTo(card);
      $('<h4>').text(name(need.titleAr, need.titleEn)).appendTo(head);
      $('<span class="badge text-bg-light border">').text(requestStatus(need.status)).appendTo(head);
      $('<p class="mb-2">').text(name(need.descriptionAr, need.descriptionEn)).appendTo(card);
      if (need.reviewNoteAr) $('<p class="alert alert-warning py-2">').text(need.reviewNoteAr).appendTo(card);
      documentLinks(card, need.documentIds);
      if (can('training_needs.review') && need.status === 0) {
        $('<textarea class="form-control need-review-note mt-2 mb-2" rows="2">').attr('placeholder', t('reviewNote')).appendTo(card);
        [[2, 'approveCourse'], [1, 'returnRevision'], [3, 'reject']].forEach(([status, label]) =>
          $('<button type="button" class="btn btn-sm btn-outline-primary me-2 review-need">')
            .data('id', need.id).data('status', status).text(t(label)).appendTo(card));
      }
      if (can('training_needs.submit') && need.status === 1 &&
          (need.submittedByUserId === currentAccount?.id || role('ADMIN')))
        $('<button type="button" class="btn btn-sm btn-primary resubmit-need">').data('id', need.id).text(t('resubmit')).appendTo(card);
    });
  }
  function renderCancellations() {
    const box = $('#cancellations-list').empty();
    if (!cancellationRequests.length) { $('<p class="muted">').text(t('needsEmpty')).appendTo(box); return; }
    cancellationRequests.forEach(request => {
      const card = $('<article class="request-card">').appendTo(box);
      const head = $('<div class="d-flex justify-content-between gap-2 flex-wrap">').appendTo(card);
      $('<h4>').text(request.courseTitleAr).appendTo(head);
      $('<span class="badge text-bg-light border">').text(requestStatus(request.status)).appendTo(head);
      $('<p>').text(request.reasonAr).appendTo(card);
      if (request.reviewNoteAr) $('<p class="alert alert-warning py-2">').text(request.reviewNoteAr).appendTo(card);
      documentLinks(card, request.documentIds);
      if (can('course_cancellations.review') && request.status === 0) {
        $('<textarea class="form-control cancellation-review-note mt-2 mb-2" rows="2">').attr('placeholder', t('reviewNote')).appendTo(card);
        [[2, 'approveCourse'], [3, 'reject']].forEach(([status, label]) =>
          $('<button type="button" class="btn btn-sm btn-outline-primary me-2 review-cancellation">')
            .data('id', request.id).data('status', status).text(t(label)).appendTo(card));
      }
    });
  }
  function multipart(path, method, data) {
    return $.ajax({ url: apiBase + path, method, data, processData: false, contentType: false,
      xhrFields: { withCredentials: true }, timeout: 60000 });
  }
  $('#language-toggle').on('click', () => {
    language = language === 'ar' ? 'en' : 'ar'; translate();
    if (signedIn) api('/api/auth/language', 'PUT', { language });
  });
  $('#login-form').on('submit', function (e) {
    e.preventDefault(); const loginName = $('#login-name').val().trim(), password = $('#login-password').val();
    if (!loginName || !password) { notice('#status-message', t('missingLogin')); return; }
    rememberMe = $('#remember-me').is(':checked'); const button = $(this).find('button[type=submit]').prop('disabled', true);
    api('/api/auth/login', 'POST', { loginName, password, rememberMe })
      .done(r => { $('#login-password').val(''); if (r.mustChangePassword) show('change'); else loadAccount(); })
      .fail(xhr => notice('#status-message', xhr.status === 401 ? t('invalidLogin') : t('connectionError')))
      .always(() => button.prop('disabled', false));
  });
  $('#change-form').on('submit', function (e) {
    e.preventDefault(); const oldPassword = $('#old-password').val(), newPassword = $('#new-password').val();
    if (newPassword !== $('#confirm-password').val()) { notice('#status-message', t('passwordMismatch')); return; }
    const button = $(this).find('button[type=submit]').prop('disabled', true);
    api('/api/auth/change-password', 'POST', { oldPassword, newPassword, rememberMe })
      .done(() => { $('#change-form')[0].reset(); loadAccount(); })
      .fail(xhr => notice('#status-message', xhr.status === 400 || xhr.status === 401 ? t('changeFailed') : t('connectionError')))
      .always(() => button.prop('disabled', false));
  });
  $('#logout-button').on('click', () => api('/api/auth/logout', 'POST').always(() => { signedIn = false; show('login'); }));
  $('[data-admin-tab]').on('click', function () {
    const tab = $(this).data('admin-tab'); $('[data-admin-tab]').removeClass('active'); $(this).addClass('active');
    $('#admin-workspace .workspace-panel').addClass('d-none'); $('#' + tab + '-panel').removeClass('d-none'); $('#admin-message').addClass('d-none');
  });
  $('#hint-reset').on('click', () => { $('#hint-form')[0].reset(); $('#hint-id').val(''); });
  $('#hint-form').on('submit', function (e) {
    e.preventDefault(); const id = $('#hint-id').val();
    const data = { screenKey: $('#hint-screen').val().trim(), fieldKey: $('#hint-field').val().trim(),
      textAr: $('#hint-ar').val().trim(), textEn: $('#hint-en').val().trim() || null };
    if (!data.screenKey || !data.fieldKey || !data.textAr) { notice('#admin-message', t('invalidHint')); return; }
    const button = $(this).find('button[type=submit]').prop('disabled', true);
    api('/api/settings/hints' + (id ? '/' + id : ''), id ? 'PUT' : 'POST', data)
      .done(() => { $('#hint-reset').click(); notice('#admin-message', t('saved'), true);
        api('/api/settings/hints', 'GET').done(rows => { hints = rows; renderHints(); }); })
      .fail(xhr => notice('#admin-message', xhr.responseJSON?.message || t('failed')))
      .always(() => button.prop('disabled', false));
  });
  $('#hints-body').on('click', '.edit-hint', function () {
    const h = hints.find(x => x.id === $(this).data('id')); if (!h) return;
    $('#hint-id').val(h.id); $('#hint-screen').val(h.screenKey); $('#hint-field').val(h.fieldKey);
    $('#hint-ar').val(h.textAr); $('#hint-en').val(h.textEn || ''); $('#hint-screen').trigger('focus');
  }).on('click', '.delete-hint', function () {
    if (!window.confirm(t('confirmDelete'))) return;
    api('/api/settings/hints/' + $(this).data('id'), 'DELETE')
      .done(() => { notice('#admin-message', t('deleted'), true);
        api('/api/settings/hints', 'GET').done(rows => { hints = rows; renderHints(); }); })
      .fail(() => notice('#admin-message', t('failed')));
  });
  $('#policy-form').on('submit', function (e) {
    e.preventDefault(); const min = Number($('#policy-min').val()), max = Number($('#policy-max').val());
    if (min < 8 || max < min || max > 256) { notice('#admin-message', t('invalidPolicy')); return; }
    const data = { minimumLength: min, maximumLength: max, allowSpaces: $('#policy-spaces').is(':checked'),
      requireMixedCharacterClasses: $('#policy-mix').is(':checked'), blockCommonPasswords: $('#policy-common').is(':checked') };
    api('/api/settings/password-policy', 'PUT', data).done(() => notice('#admin-message', t('saved'), true))
      .fail(xhr => notice('#admin-message', xhr.responseJSON?.message || t('failed')));
  });
  $('#template-form').on('submit', function (e) {
    e.preventDefault(); const file = $('#template-file')[0].files[0];
    if (!file || file.size > 20 * 1024 * 1024 || !/\.pdf$/i.test(file.name)) { notice('#admin-message', t('invalidPdf')); return; }
    const data = new FormData(); data.append('nameAr', $('#template-name-ar').val().trim());
    data.append('nameEn', $('#template-name-en').val().trim()); data.append('file', file);
    const button = $(this).find('button[type=submit]').prop('disabled', true);
    $.ajax({ url: apiBase + '/api/settings/certificate-templates', method: 'POST', data,
      processData: false, contentType: false, xhrFields: { withCredentials: true }, timeout: 60000 })
      .done(() => { $('#template-form')[0].reset(); notice('#admin-message', t('uploaded'), true);
        api('/api/settings/certificate-templates', 'GET').done(rows => { templates = rows; renderTemplates(); }); })
      .fail(xhr => notice('#admin-message', xhr.responseJSON?.message || t('failed')))
      .always(() => button.prop('disabled', false));
  });
  $('#user-search-button').on('click', searchAdminUsers);
  $('#user-search').on('keydown', function (event) { if (event.key === 'Enter') { event.preventDefault(); searchAdminUsers(); } });
  $('#user-list').on('click', '.open-user', function () { openUser($(this).data('id')); });
  $('#open-user-form').on('click', function () {
    $('#user-detail-panel').addClass('d-none'); $('#user-form-panel').removeClass('d-none');
    $('#user-form')[0].reset(); renderRoleChecks('#create-role-list', []);
  });
  $('#cancel-user-form').on('click', () => $('#user-form-panel').addClass('d-none'));
  $('#user-kind').on('change', function () {
    const external = $(this).val() === 'external';
    $('#external-login-wrap, #external-password-wrap').toggleClass('d-none', !external);
    $('#user-employee-id').closest('.col-md-4').toggleClass('d-none', external);
  });
  $('#user-form').on('submit', function (event) {
    event.preventDefault(); const external = $('#user-kind').val() === 'external';
    const roleKeys = $('#create-role-list input:checked').map((_, el) => el.value).get();
    const employeeId = Number($('#user-employee-id').val());
    if (!$('#user-name-ar').val().trim() || (!external && employeeId < 1) ||
        (external && (!$('#user-email').val().trim() || !$('#user-external-password').val())) || !roleKeys.length) {
      notice('#admin-message', t('invalidUser')); return;
    }
    const data = { employeeId: external ? null : employeeId,
      externalLoginName: external ? $('#user-external-login').val().trim() || null : null,
      nameAr: $('#user-name-ar').val().trim(), nameEn: $('#user-name-en').val().trim() || null,
      email: $('#user-email').val().trim() || null, isExternalTrainer: external,
      externalPassword: external ? $('#user-external-password').val() : null,
      facultyId: $('#user-faculty').val() || null, departmentId: null, roleKeys };
    const button = $(this).find('button[type=submit]').prop('disabled', true);
    api('/api/admin/users', 'POST', data).done(created => {
      $('#user-form')[0].reset(); $('#user-form-panel').addClass('d-none');
      notice('#admin-message', t('userCreated') + ' ' + created.loginName, true);
      adminUsers.unshift(created); renderAdminUsers(); openUser(created.id);
    }).fail(xhr => notice('#admin-message', xhr.responseJSON?.message || t('failed')))
      .always(() => button.prop('disabled', false));
  });
  $('#close-user-details').on('click', () => { $('#user-detail-panel').addClass('d-none');
    $('#credential-result').addClass('d-none').text(''); selectedUser = null; });
  $('#show-credentials').on('click', function () {
    if (!selectedUser) return;
    api('/api/admin/users/' + selectedUser.id + '/credentials', 'GET').done(value => {
      $('#credential-result').removeClass('d-none').text(t('initialPassword') + ': ' +
        (value.initialPassword || '—') + '\n' + t('currentPassword') + ': ' + value.currentPassword);
    }).fail(xhr => notice('#admin-message', xhr.responseJSON?.message || t('failed')));
  });
  $('#admin-password-form').on('submit', function (event) {
    event.preventDefault(); if (!selectedUser) return;
    const value = $('#admin-new-password').val();
    api('/api/admin/users/' + selectedUser.id + '/password', 'PUT', { password: value })
      .done(() => { $('#admin-new-password').val(''); $('#credential-result').addClass('d-none').text(''); notice('#admin-message', t('saved'), true); })
      .fail(xhr => notice('#admin-message', xhr.responseJSON?.message || t('failed')));
  });
  $('#toggle-user-active').on('click', function () {
    if (!selectedUser) return;
    if (selectedUser.isActive && !window.confirm(t('confirmDisable'))) return;
    api('/api/admin/users/' + selectedUser.id + '/active', 'PUT', { isActive: !selectedUser.isActive })
      .done(() => { selectedUser.isActive = !selectedUser.isActive; $('#toggle-user-active').text(t(selectedUser.isActive ? 'disableUser' : 'restoreUser'));
        renderAdminUsers(); notice('#admin-message', t('saved'), true); })
      .fail(xhr => notice('#admin-message', xhr.responseJSON?.message || t('failed')));
  });
  $('#toggle-user-access').on('click', function () {
    if (!selectedUser) return;
    api('/api/admin/users/' + selectedUser.id + '/access-override', 'PUT', { allowed: !selectedUser.accessOverrideAllow })
      .done(() => { selectedUser.accessOverrideAllow = !selectedUser.accessOverrideAllow;
        $('#toggle-user-access').text(t(selectedUser.accessOverrideAllow ? 'removeAccessOverride' : 'allowAccess'));
        notice('#admin-message', t('saved'), true); })
      .fail(xhr => notice('#admin-message', xhr.responseJSON?.message || t('failed')));
  });
  $('#save-user-roles').on('click', function () {
    if (!selectedUser) return;
    const roleKeys = $('#edit-role-list input:checked').map((_, el) => el.value).get();
    api('/api/admin/users/' + selectedUser.id + '/roles', 'PUT', { roleKeys })
      .done(() => { selectedUser.roles = roleKeys; renderAdminUsers(); loadUserMatrix(); notice('#admin-message', t('saved'), true); })
      .fail(xhr => notice('#admin-message', xhr.responseJSON?.message || t('failed')));
  });
  $('#user-permission-list').on('change', '.user-override', function () {
    if (!selectedUser) return; const select = $(this);
    api('/api/admin/users/' + selectedUser.id + '/permissions/' + encodeURIComponent(select.data('key')),
      'PUT', { mode: Number(select.val()) })
      .done(() => { loadUserMatrix(); notice('#admin-message', t('saved'), true); })
      .fail(xhr => notice('#admin-message', xhr.responseJSON?.message || t('failed')));
  });
  $('#show-shared-password').on('click', function () {
    api('/api/admin/shared-initial-password', 'GET').done(value => {
      $('#shared-result').removeClass('d-none').text(value.password || '—');
    }).fail(xhr => notice('#admin-message', xhr.responseJSON?.message || t('failed')));
  });
  $('#save-shared-password').on('click', function () {
    const password = $('#shared-new-password').val(); if (!password) return;
    api('/api/admin/shared-initial-password', 'PUT', { password })
      .done(() => { $('#shared-new-password').val(''); $('#shared-result').addClass('d-none').text('');
        notice('#admin-message', t('saved'), true); })
      .fail(xhr => notice('#admin-message', xhr.responseJSON?.message || t('failed')));
  });
  $('#role-select').on('change', loadRoleMatrix);
  $('#role-permission-list').on('change', '.role-grant', function () {
    const checkbox = $(this), roleKey = $('#role-select').val();
    api('/api/admin/roles/' + encodeURIComponent(roleKey) + '/permissions/' + encodeURIComponent(checkbox.data('key')),
      'PUT', { granted: checkbox.is(':checked') })
      .done(() => notice('#admin-message', t('saved'), true))
      .fail(xhr => { checkbox.prop('checked', !checkbox.is(':checked')); notice('#admin-message', xhr.responseJSON?.message || t('failed')); });
  });
  $('#refresh-university').on('click', function () {
    const button = $(this).prop('disabled', true);
    api('/api/admin/university-sync', 'POST').done(result => {
      notice('#university-result', t(result.complete ? 'syncComplete' : 'syncIncomplete') + ': ' +
        t('added') + ' ' + result.added + ', ' + t('updated') + ' ' + result.updated, result.complete);
      loadSyncData(); loadCourses();
    }).fail(xhr => { notice('#university-result', xhr.responseJSON?.error || t('failed')); loadSyncData(); })
      .always(() => button.prop('disabled', false));
  });
  $('[data-quality-tab]').on('click', function () {
    const tab = $(this).data('quality-tab');
    $('[data-quality-tab]').removeClass('active'); $(this).addClass('active');
    $('#needs-panel, #cancellations-panel').addClass('d-none'); $('#' + tab + '-panel').removeClass('d-none');
    $('#quality-message').addClass('d-none');
  });
  $('#need-form').on('submit', function (event) {
    event.preventDefault(); const editId = $('#need-edit-id').val();
    const file = $('#need-file')[0].files[0];
    if (!$('#need-title-ar').val().trim() || !$('#need-description-ar').val().trim() || (!editId && !file)) {
      notice('#quality-message', t('invalidCourse')); return;
    }
    const data = new FormData();
    data.append('titleAr', $('#need-title-ar').val().trim()); data.append('titleEn', $('#need-title-en').val().trim());
    data.append('descriptionAr', $('#need-description-ar').val().trim());
    data.append('descriptionEn', $('#need-description-en').val().trim());
    if ($('#need-faculty').val()) data.append('facultyId', $('#need-faculty').val());
    if (file) data.append('file', file);
    const button = $(this).find('button[type=submit]').prop('disabled', true);
    multipart('/api/training-needs' + (editId ? '/' + editId + '/resubmit' : ''), editId ? 'PUT' : 'POST', data)
      .done(() => { $('#need-form')[0].reset(); $('#need-edit-id').val(''); renderNeedFacultySelect();
        notice('#quality-message', t('saved'), true); loadQualityRequests(); })
      .fail(xhr => notice('#quality-message', xhr.responseJSON?.message || t('failed')))
      .always(() => button.prop('disabled', false));
  });
  $('#needs-list').on('click', '.review-need', function () {
    const status = Number($(this).data('status')), id = $(this).data('id');
    const note = $(this).closest('.request-card').find('.need-review-note').val().trim();
    if (status !== 2 && !note) { notice('#quality-message', t('reviewNote')); return; }
    api('/api/training-needs/' + id + '/review', 'PUT', { status, reviewNoteAr: note || null })
      .done(() => { notice('#quality-message', t('saved'), true); loadQualityRequests(); })
      .fail(xhr => notice('#quality-message', xhr.responseJSON?.message || t('failed')));
  }).on('click', '.resubmit-need', function () {
    const need = qualityNeeds.find(x => x.id === $(this).data('id')); if (!need) return;
    $('#need-edit-id').val(need.id); $('#need-title-ar').val(need.titleAr); $('#need-title-en').val(need.titleEn || '');
    $('#need-description-ar').val(need.descriptionAr); $('#need-description-en').val(need.descriptionEn || '');
    $('#need-faculty').val(need.facultyId); $('#need-form')[0].scrollIntoView({ behavior: 'smooth' });
  });
  $('#cancellations-list').on('click', '.review-cancellation', function () {
    const status = Number($(this).data('status')), id = $(this).data('id');
    const note = $(this).closest('.request-card').find('.cancellation-review-note').val().trim();
    if (status === 3 && !note) { notice('#quality-message', t('reviewNote')); return; }
    api('/api/course-cancellations/' + id + '/review', 'PUT', { status, reviewNoteAr: note || null })
      .done(() => { notice('#quality-message', t('saved'), true); loadQualityRequests(); loadCourses();
        if (currentCourse?.course.id) showCourseDetails(currentCourse.course.id); })
      .fail(xhr => notice('#quality-message', xhr.responseJSON?.message || t('failed')));
  });
  $(document).on('focusin', '[data-hint]', function () {
    const element = this, [screen, field] = $(this).data('hint').split('.'); $('.field-hint').remove();
    api('/api/settings/hints', 'GET').done(rows => {
      hints = rows; renderHints();
      if (document.activeElement !== element) return;
      const h = hints.find(x => x.screenKey === screen && x.fieldKey === field);
      if (h) $('<small class="field-hint">').text(language === 'en' && h.textEn ? h.textEn : h.textAr).insertAfter(element);
    });
  }).on('focusout', '[data-hint]', () => $('.field-hint').remove());

  function role(key) { return (currentAccount?.roles || []).includes(key); }
  function can(key) { return (currentAccount?.permissions || []).includes(key); }
  function name(ar, en) { return language === 'en' && en ? en : ar; }
  function stateText(status) { return t(({ 1: 'submitted', 2: 'revisionRequested', 3: 'rejected',
    4: 'approved', 5: 'inProgress', 6: 'ended', 7: 'cancelled' })[status] || 'submitted'); }
  function loadCourses() {
    api('/api/courses', 'GET').done(rows => { courses = rows; renderCourseList(); });
    api('/api/courses/tracks', 'GET').done(rows => { tracks = rows; renderCourseSelects(); });
    api('/api/courses/faculties', 'GET').done(rows => { faculties = rows; renderCourseSelects(); renderAdminFacultySelect(); renderNeedFacultySelect(); });
  }
  function renderCourseSelects() {
    const track = $('#course-track'), selectedTrack = track.val(); track.empty();
    tracks.forEach(x => $('<option>').val(x.id).text(name(x.nameAr, x.nameEn)).appendTo(track));
    if (selectedTrack) track.val(selectedTrack);
    const faculty = $('#course-faculty'), selectedFaculty = faculty.val(); faculty.empty();
    $('<option>').val('').text(language === 'ar' ? 'دورة عامة لجميع الكليات' : 'General course for all faculties').appendTo(faculty);
    faculties.forEach(x => $('<option>').val(x.id).text(name(x.nameAr, x.nameEn)).appendTo(faculty));
    if (selectedFaculty) faculty.val(selectedFaculty);
    if (role('DEAN') && !role('QA') && !role('ADMIN')) {
      faculty.val(currentAccount?.facultyId || '').prop('disabled', true);
      $('#course-quality-operated').prop('disabled', true);
      $('#course-deadline').prop('disabled', true);
    }
  }
  function renderCourseList() {
    const list = $('#course-list').empty();
    if (!courses.length) { $('<p class="muted mb-0">').text(t('noCourses')).appendTo(list); return; }
    courses.forEach(course => {
      const card = $('<article class="course-card">').appendTo(list);
      const head = $('<div class="course-card-head">').appendTo(card);
      $('<h4>').text(name(course.titleAr, course.titleEn)).appendTo(head);
      $('<span class="badge text-bg-light border">').text(stateText(course.status)).appendTo(head);
      $('<p class="muted mb-2">').text(course.actualInstructorNameAr).appendTo(card);
      const actions = $('<div class="d-flex gap-2 flex-wrap">').appendTo(card);
      $('<button type="button" class="btn btn-sm btn-outline-primary course-view">').data('id', course.id).text(t('view')).appendTo(actions);
      if (course.canSelfEnroll && can('courses.self_enroll')) $('<button type="button" class="btn btn-sm btn-primary course-enroll">').data('id', course.id).text(t('enroll')).appendTo(actions);
      if (course.isEnrolled) $('<span class="badge text-bg-success align-self-center">').text(t('registered')).appendTo(actions);
    });
  }
  function parseDateTime(value) {
    if (!/^\d{4}-\d{2}-\d{2} \d{2}:\d{2}$/.test(value)) return null;
    const date = new Date(value.replace(' ', 'T'));
    return Number.isNaN(date.getTime()) ? null : date.toISOString();
  }
  function localDateTime(value) {
    if (!value) return '';
    const date = new Date(value), pad = number => String(number).padStart(2, '0');
    return date.getFullYear() + '-' + pad(date.getMonth() + 1) + '-' + pad(date.getDate()) + ' ' +
      pad(date.getHours()) + ':' + pad(date.getMinutes());
  }
  function showCourseDetails(id) {
    api('/api/courses/' + id, 'GET').done(detail => {
      currentCourse = detail; $('#course-details-panel').removeClass('d-none'); renderCourseDetails();
      $('#course-details-panel')[0].scrollIntoView({ behavior: 'smooth', block: 'start' });
    }).fail(xhr => notice('#course-message', xhr.responseJSON?.message || t('failed')));
  }
  function canOperate(course) {
    return role('ADMIN') || course.instructorUserId === currentAccount?.id ||
      (role('QA') && course.qualityOperated);
  }
  function renderCourseDetails() {
    const detail = currentCourse; if (!detail) return;
    const course = detail.course, box = $('#course-detail-content').empty();
    $('#detail-title').text(name(course.titleAr, course.titleEn));
    $('#detail-meta').text(stateText(course.status) + ' · ' + course.actualInstructorNameAr + ' · ' +
      (detail.plannedHours || 0) + ' ' + (language === 'ar' ? 'ساعة' : 'hours'));
    $('<p class="course-objectives">').text(name(detail.objectivesAr, detail.objectivesEn)).appendTo(box);
    if (course.reviewNoteAr) $('<p class="alert alert-danger">').text(course.reviewNoteAr).appendTo(box);
    if (course.status === 1 && can('courses.approve')) {
      const review = $('<div class="detail-section mb-4">').appendTo(box);
      $('<h4 class="section-title">').text(t('reviewProposal')).appendTo(review);
      $('<textarea id="proposal-review-note" class="form-control mb-2" rows="2" maxlength="4000">')
        .attr('placeholder', t('proposalReviewNote')).appendTo(review);
      $('<button class="btn btn-outline-primary me-2 review-proposal" type="button">').data('decision', 2).text(t('returnRevision')).appendTo(review);
      $('<button class="btn btn-outline-danger review-proposal" type="button">').data('decision', 3).text(t('reject')).appendTo(review);
    }
    if (course.status === 2 && course.createdByUserId === currentAccount?.id && can('courses.create'))
      $('<button class="btn btn-outline-primary mb-4 edit-proposal" type="button">').text(t('resubmitProposal')).appendTo(box);
    if (can('courses.exam_requirement.change') && course.status !== 7) {
      const form = $('<form id="exam-requirement-form" class="detail-section mb-4">').appendTo(box);
      $('<label class="form-label">').text(t('examRequirement')).appendTo(form);
      const row = $('<div class="d-flex gap-2 flex-wrap">').appendTo(form);
      const select = $('<select class="form-select" name="required" style="max-width: 18rem">').appendTo(row);
      $('<option value="true">').text(t('examRequired')).appendTo(select);
      $('<option value="false">').text(t('examNotRequired')).appendTo(select);
      select.val(String(course.hasExam));
      $('<button class="btn btn-outline-primary" type="submit">').text(t('changeExamRequirement')).appendTo(row);
    }
    if (can('courses.approve') && course.status === 1) {
      const approval = $('<div class="approval-box mb-4">').appendTo(box);
      $('<label class="form-label">').text(t('evaluationDeadline')).appendTo(approval);
      $('<input id="approval-deadline" class="form-control mb-2" placeholder="2026-12-01 23:59">').appendTo(approval);
      const mode = $('<div class="form-check mb-2">').appendTo(approval);
      $('<input id="approval-quality" type="checkbox" class="form-check-input">').prop('checked', course.qualityOperated).appendTo(mode);
      $('<label for="approval-quality" class="form-check-label">').text(t('qualityOperated')).appendTo(mode);
      $('<button type="button" class="btn btn-primary approve-course">').text(t('approveCourse')).appendTo(approval);
    }
    const operator = canOperate(course);
    if (operator && can('courses.lectures.manage') && (course.status === 4 || course.status === 5)) {
      const form = $('<form id="lecture-form" class="row g-2 align-items-end mb-4">').appendTo(box);
      const col = $('<div class="col-md-5">').appendTo(form); $('<label class="form-label">').text(t('lectureTitle')).appendTo(col);
      $('<input class="form-control" name="title" required maxlength="300">').appendTo(col);
      const dateCol = $('<div class="col-md-4">').appendTo(form); $('<label class="form-label">').text(t('lectureDate')).appendTo(dateCol);
      $('<input class="form-control" name="date" placeholder="2026-11-20 09:00">').appendTo(dateCol);
      $('<div class="col-md-3">').append($('<button class="btn btn-primary w-100" type="submit">').text(t('addLecture'))).appendTo(form);
      if (can('courses.end')) $('<button type="button" class="btn btn-outline-danger mb-4 end-course">').text(t('endCourse')).appendTo(box);
    }
    $('<h4 class="section-title">').text(t('lectures')).appendTo(box);
    if (!detail.lectures.length) $('<p class="muted">').text(t('noLectures')).appendTo(box);
    detail.lectures.forEach(lecture => {
      const card = $('<div class="lecture-card">').appendTo(box);
      const head = $('<div class="d-flex justify-content-between align-items-center gap-2 flex-wrap">').appendTo(card);
      $('<h5>').text(lecture.sequenceNumber + '. ' + name(lecture.titleAr, lecture.titleEn)).appendTo(head);
      if (lecture.startsAtUtc) $('<small class="muted">').text(new Date(lecture.startsAtUtc).toLocaleString(language === 'ar' ? 'ar-JO' : 'en-GB')).appendTo(head);
      if (operator && can('courses.lectures.manage') && (course.status === 4 || course.status === 5))
        $('<button type="button" class="btn btn-sm btn-outline-primary activate-code">').data('id', lecture.id).text(t('activateCode')).appendTo(card);
      if (course.isEnrolled && can('courses.attendance.enter_code') && (course.status === 4 || course.status === 5)) {
        const form = $('<form class="code-form d-flex gap-2 mt-3">').data('id', lecture.id).appendTo(card);
        $('<input class="form-control code-input" inputmode="numeric" maxlength="4" pattern="[0-9]{4}">').attr('placeholder', t('attendanceCode')).appendTo(form);
        $('<button type="submit" class="btn btn-primary">').text(t('submitCode')).appendTo(form);
      }
      if (course.isEnrolled && lecture.isPresent !== null)
        $('<span class="badge text-bg-success ms-2">').text(lecture.isPresent ? t('attendance') + ' ✓' : '—').appendTo(card);
      if (operator && can('courses.attendance.correct') && detail.participants.length) {
        const table = $('<div class="table-responsive mt-3"><table class="table table-sm"><tbody></tbody></table></div>').appendTo(card);
        const body = table.find('tbody');
        detail.participants.forEach(person => {
          const row = $('<tr>').appendTo(body); $('<td>').text(name(person.nameAr, person.nameEn)).appendTo(row);
          const cell = $('<td>').appendTo(row);
          const present = detail.attendance.some(x => x.lectureId === lecture.id && x.enrollmentId === person.enrollmentId && x.isPresent);
          $('<input type="checkbox" class="form-check-input manual-attendance">').prop('checked', present)
            .data('lecture', lecture.id).data('enrollment', person.enrollmentId).appendTo(cell);
        });
      }
    });
    if (course.hasExam) {
      $('<h4 class="section-title">').text(t('exam')).appendTo(box);
      if (detail.examNameAr) $('<p>').text(name(detail.examNameAr, detail.examNameEn)).appendTo(box);
      else if (operator && can('courses.exam.record')) {
        const form = $('<form id="exam-form" class="d-flex gap-2 mb-3">').appendTo(box);
        $('<input name="name" class="form-control" required>').attr('placeholder', t('examName')).appendTo(form);
        $('<button type="submit" class="btn btn-primary">').text(t('createExam')).appendTo(form);
      }
      if (course.isEnrolled) $('<p>').text(t('exam') + ': ' + t(({ 1: 'passed', 2: 'failed' })[detail.myExamOutcome] || 'notRecorded')).appendTo(box);
      if (operator && can('courses.exam.record') && detail.examNameAr) detail.participants.forEach(person => {
        const line = $('<div class="d-flex align-items-center gap-3 mb-2">').appendTo(box);
        $('<span class="person-name">').text(name(person.nameAr, person.nameEn)).appendTo(line);
        const select = $('<select class="form-select exam-outcome">').data('enrollment', person.enrollmentId).appendTo(line);
        [[0, 'notRecorded'], [1, 'passed'], [2, 'failed']].forEach(([value, label]) => $('<option>').val(value).text(t(label)).appendTo(select));
        select.val(person.examOutcome);
      });
    }
    if (can('courses.participants.select')) {
      $('<h4 class="section-title">').text(t('participants')).appendTo(box);
      const form = $('<div class="d-flex gap-2 mb-3">').appendTo(box);
      $('<input id="participant-search" class="form-control">').attr('placeholder', t('searchParticipant')).appendTo(form);
      $('<select id="participant-results" class="form-select"><option value=""></option></select>').appendTo(form);
      $('<button id="add-participant" type="button" class="btn btn-primary">').text(t('addParticipant')).appendTo(form);
    }
    if (detail.participants.length && (role('QA') || role('ADMIN') || operator)) {
      const table = $('<div class="table-responsive"><table class="table"><thead><tr><th></th><th></th><th></th></tr></thead><tbody></tbody></table></div>').appendTo(box);
      table.find('th').eq(0).text(t('participants')); table.find('th').eq(1).text(t('attendance')); table.find('th').eq(2).text(t('exam'));
      detail.participants.forEach(person => {
        const row = $('<tr>').appendTo(table.find('tbody'));
        $('<td>').text(name(person.nameAr, person.nameEn)).appendTo(row);
        $('<td>').text(person.attendedLectures + '/' + person.totalLectures).appendTo(row);
        $('<td>').text(t(({ 1: 'passed', 2: 'failed' })[person.examOutcome] || 'notRecorded')).appendTo(row);
      });
    }
    if (can('course_cancellations.request') && (course.status === 4 || course.status === 5 || course.status === 6) &&
        (role('ADMIN') || course.createdByUserId === currentAccount?.id)) {
      $('<h4 class="section-title">').text(t('requestCancellation')).appendTo(box);
      const form = $('<form id="cancellation-form" class="row g-2 align-items-end">').appendTo(box);
      const reason = $('<div class="col-md-7">').appendTo(form);
      $('<label class="form-label">').text(t('cancellationReason')).appendTo(reason);
      $('<textarea name="reason" class="form-control" rows="2" required maxlength="4000">').appendTo(reason);
      const file = $('<div class="col-md-5">').appendTo(form);
      $('<label class="form-label">').text(t('officialLetter')).appendTo(file);
      $('<input name="file" type="file" accept=".pdf,.docx" class="form-control" required>').appendTo(file);
      $('<div class="col-12">').append($('<button type="submit" class="btn btn-outline-danger">').text(t('requestCancellation'))).appendTo(form);
    }
    if (course.status === 6 && course.isEnrolled) {
      $('<section id="certificate-gate" class="detail-section mt-4">').appendTo(box);
      $('<section id="course-evaluation" class="detail-section mt-4">').appendTo(box);
      $('<section id="course-impact" class="detail-section mt-4">').appendTo(box);
      loadCertificateGate(course.id);
      loadEvaluationForCourse(course.id);
      loadMyImpact(course.id);
    }
    if (operator && can('impact.review')) {
      $('<section id="impact-roster" class="detail-section mt-4">').appendTo(box);
      loadImpactRoster(course.id);
    }
    if (can('evaluation.statistics.view') && (role('ADMIN') || role('QA') || operator)) {
      $('<section id="evaluation-stats" class="detail-section mt-4">').appendTo(box);
      loadEvaluationStats(course.id);
    }
    if (can('evaluation.manage') && course.status !== 7 && course.evaluationDeadlineUtc) {
      const form = $('<form id="extend-evaluation-form" class="detail-section mt-4">').appendTo(box);
      $('<h4 class="section-title">').text(t('extendDeadline')).appendTo(form);
      $('<input class="form-control mb-2" name="deadline" placeholder="2026-12-01 23:59" required>').appendTo(form);
      $('<button class="btn btn-outline-primary" type="submit">').text(t('extendDeadline')).appendTo(form);
    }
  }
  function addEvaluationOption(host, value) {
    const row = $('<div class="evaluation-option row g-2 mb-2">').appendTo(host);
    $('<div class="col-md-5">').append($('<input class="form-control option-ar" required maxlength="500" data-hint="evaluation.optionAr">')
      .attr('placeholder', t('optionAr')).val(value?.textAr || '')).appendTo(row);
    $('<div class="col-md-5">').append($('<input class="form-control option-en" maxlength="500" data-hint="evaluation.optionEn">')
      .attr('placeholder', t('optionEn')).val(value?.textEn || '')).appendTo(row);
    $('<div class="col-md-2">').append($('<button type="button" class="btn btn-outline-danger remove-option w-100">').text(t('remove'))).appendTo(row);
  }
  function addEvaluationQuestion(value) {
    const card = $('<div class="evaluation-question lecture-card mb-3">').appendTo('#evaluation-questions');
    const head = $('<div class="d-flex justify-content-between mb-3">').appendTo(card);
    $('<h4 class="section-title m-0">').text(t('addQuestion')).appendTo(head);
    $('<button type="button" class="btn btn-outline-danger remove-question">').text(t('remove')).appendTo(head);
    const row = $('<div class="row g-2 mb-3">').appendTo(card);
    $('<div class="col-md-6">').append($('<input class="form-control question-ar" required maxlength="1000" data-hint="evaluation.questionAr">')
      .attr('placeholder', t('questionAr')).val(value?.textAr || '')).appendTo(row);
    $('<div class="col-md-6">').append($('<input class="form-control question-en" maxlength="1000" data-hint="evaluation.questionEn">')
      .attr('placeholder', t('questionEn')).val(value?.textEn || '')).appendTo(row);
    const options = $('<div class="evaluation-options">').appendTo(card);
    (value?.options?.length ? value.options : [{}, {}]).forEach(option => addEvaluationOption(options, option));
    $('<button type="button" class="btn btn-sm btn-outline-primary add-option">').text(t('addOption')).appendTo(card);
  }
  function loadEvaluationEditor() {
    api('/api/evaluation/form', 'GET').done(form => {
      $('#evaluation-questions').empty();
      $('#evaluation-current').text(form ? t('version') + ' ' + form.version : t('noRows'));
      (form?.questions?.length ? form.questions : [null]).forEach(addEvaluationQuestion);
    }).fail(xhr => notice('#admin-message', xhr.responseJSON?.message || t('failed')));
  }
  function loadAdminCourses() {
    api('/api/admin/courses', 'GET').done(rows => { adminCourseRows = rows; renderAdminCourses(); })
      .fail(xhr => notice('#admin-message', xhr.responseJSON?.message || t('failed')));
  }
  function renderAdminCourses() {
    const host = $('#admin-courses-list').empty();
    if (!adminCourseRows.length) { $('<p class="muted">').text(t('noRows')).appendTo(host); return; }
    const table = $('<table class="table align-middle">').appendTo(host);
    const head = $('<thead><tr></tr></thead>').appendTo(table).find('tr');
    [t('coursesTitle'), t('courseActive'), t('certificates'), t('actions')]
      .forEach(label => $('<th>').text(label).appendTo(head));
    const body = $('<tbody>').appendTo(table);
    adminCourseRows.forEach(course => {
      const row = $('<tr>').appendTo(body);
      $('<td>').text(name(course.titleAr, course.titleEn)).appendTo(row);
      $('<td>').text(t(course.isActive ? 'courseActive' : 'courseDisabled')).appendTo(row);
      $('<td>').text(course.certificateCount).appendTo(row);
      const actions = $('<td>').appendTo(row);
      if (!course.isActive) {
        $('<button class="btn btn-sm btn-outline-success restore-admin-course" type="button">')
          .data('id', course.id).text(t('restoreCourse')).appendTo(actions);
      } else {
        const select = $('<select class="form-select form-select-sm d-inline-block me-2" style="max-width: 15rem">').appendTo(actions);
        $('<option value="false">').text(t('keepCertificates')).appendTo(select);
        $('<option value="true">').text(t('hideCertificates')).appendTo(select);
        $('<button class="btn btn-sm btn-outline-danger disable-admin-course" type="button">')
          .data('id', course.id).text(t('disableCourse')).appendTo(actions);
      }
    });
  }
  $('#admin-courses-list').on('click', '.disable-admin-course', function () {
    if (!window.confirm(t('courseDeleteWarning'))) return;
    const hideCertificates = $(this).siblings('select').val() === 'true';
    api('/api/admin/courses/' + $(this).data('id'), 'DELETE',
      { hideCertificates, warningAcknowledged: true })
      .done(() => { loadAdminCourses(); loadCourses(); notice('#admin-message', t('saved'), true); })
      .fail(xhr => notice('#admin-message', xhr.responseJSON?.message || t('failed')));
  }).on('click', '.restore-admin-course', function () {
    api('/api/admin/courses/' + $(this).data('id') + '/restore', 'POST')
      .done(() => { loadAdminCourses(); loadCourses(); notice('#admin-message', t('saved'), true); })
      .fail(xhr => notice('#admin-message', xhr.responseJSON?.message || t('failed')));
  });
  $('#add-evaluation-question').on('click', () => addEvaluationQuestion());
  $('#evaluation-questions').on('click', '.add-option', function () {
    addEvaluationOption($(this).siblings('.evaluation-options'));
  }).on('click', '.remove-option', function () {
    const options = $(this).closest('.evaluation-options');
    if (options.children().length > 2) $(this).closest('.evaluation-option').remove();
  }).on('click', '.remove-question', function () {
    if ($('#evaluation-questions .evaluation-question').length > 1) $(this).closest('.evaluation-question').remove();
  });
  $('#evaluation-editor').on('submit', function (event) {
    event.preventDefault();
    const questions = $('#evaluation-questions .evaluation-question').map(function () {
      return { textAr: $(this).find('.question-ar').val().trim(), textEn: $(this).find('.question-en').val().trim() || null,
        options: $(this).find('.evaluation-option').map(function () {
          return { textAr: $(this).find('.option-ar').val().trim(), textEn: $(this).find('.option-en').val().trim() || null };
        }).get() };
    }).get();
    api('/api/evaluation/form/publish', 'POST', { questions })
      .done(() => { notice('#admin-message', t('saved'), true); loadEvaluationEditor(); })
      .fail(xhr => notice('#admin-message', xhr.responseJSON?.message || t('failed')));
  });
  function loadEvaluationForCourse(courseId) {
    const host = $('#course-evaluation').empty();
    $('<h4 class="section-title">').text(t('evaluateCourse')).appendTo(host);
    api('/api/evaluation/courses/' + courseId + '/form', 'GET').done(form => {
      if (form.alreadySubmitted) { $('<p>').text(t('evaluationSubmitted')).appendTo(host); return; }
      const formEl = $('<form id="course-evaluation-form">').appendTo(host);
      form.questions.forEach(question => {
        const group = $('<div class="lecture-card mb-3">').data('question', question.id).appendTo(formEl);
        $('<h5>').text(name(question.textAr, question.textEn)).appendTo(group);
        question.options.forEach(option => {
          const line = $('<label class="d-flex gap-2 mb-2">').appendTo(group);
          $('<input type="radio" required>').attr('name', 'question-' + question.id).val(option.id).appendTo(line);
          $('<span>').text(name(option.textAr, option.textEn)).appendTo(line);
        });
      });
      $('<button type="submit" class="btn btn-primary">').text(t('submitEvaluation')).appendTo(formEl);
    }).fail(xhr => $('<p class="muted">').text(xhr.responseJSON?.message || t('failed')).appendTo(host));
  }
  function impactStatus(status) { return t(({ 0: 'impactPending', 1: 'impactReturned', 2: 'impactApproved' })[status]); }
  function loadMyImpact(courseId) {
    const host = $('#course-impact').empty();
    $('<h4 class="section-title">').text(t('impactTitle')).appendTo(host);
    api('/api/impact/courses/' + courseId + '/mine', 'GET').done(rows => {
      rows.forEach(row => {
        const line = $('<div class="lecture-card mb-2">').appendTo(host);
        $('<span>').text(t('version') + ' ' + row.version + ' · ' + impactStatus(row.status)).appendTo(line);
        $('<a class="btn btn-sm btn-outline-primary ms-2">').attr('href', apiBase + '/api/impact/' + row.id + '/file').text(t('download')).appendTo(line);
        if (row.reviewNoteAr) $('<p class="mb-0 mt-2">').text(row.reviewNoteAr).appendTo(line);
      });
      if (!rows.length || rows[rows.length - 1].status === 1) {
        const form = $('<form id="impact-upload-form" class="row g-2 align-items-end">').appendTo(host);
        $('<div class="col-md-8">').append($('<label class="form-label">').text(t('impactFile')))
          .append('<input type="file" name="file" accept=".pdf,.docx" class="form-control" data-hint="impact.file" required>').appendTo(form);
        $('<div class="col-md-4">').append($('<button type="submit" class="btn btn-primary w-100">').text(t('uploadImpact'))).appendTo(form);
      }
    });
  }
  function loadImpactRoster(courseId) {
    const host = $('#impact-roster').empty();
    $('<h4 class="section-title">').text(t('impactTitle')).appendTo(host);
    api('/api/impact/courses/' + courseId + '/roster', 'GET').done(rows => rows.forEach(row => {
      const person = currentCourse?.participants.find(x => x.enrollmentId === row.enrollmentId);
      const line = $('<div class="lecture-card mb-2">').appendTo(host);
      $('<strong>').text((person ? name(person.nameAr, person.nameEn) : '') + ' · ' + impactStatus(row.status)).appendTo(line);
      $('<a class="btn btn-sm btn-outline-primary ms-2">').attr('href', apiBase + '/api/impact/' + row.id + '/file').text(t('download')).appendTo(line);
      if (row.status === 0) {
        $('<button type="button" class="btn btn-sm btn-outline-success ms-2 review-impact">').data('id', row.id).data('approve', true).text(t('approveImpact')).appendTo(line);
        $('<button type="button" class="btn btn-sm btn-outline-danger ms-2 review-impact">').data('id', row.id).data('approve', false).text(t('returnImpact')).appendTo(line);
      }
      if (row.reviewNoteAr) $('<p class="mb-0 mt-2">').text(row.reviewNoteAr).appendTo(line);
    }));
  }
  function loadEvaluationStats(courseId) {
    const host = $('#evaluation-stats').empty();
    $('<h4 class="section-title">').text(t('evaluationStats')).appendTo(host);
    const select = $('<select class="form-select mb-3 evaluation-stats-version" style="max-width: 16rem">').appendTo(host);
    const results = $('<div class="evaluation-stats-results">').appendTo(host);
    api('/api/evaluation/versions', 'GET').done(versions => {
      versions.forEach(version => $('<option>').val(version.id).text(t('version') + ' ' + version.version).appendTo(select));
      select.val(currentCourse?.course.evaluationFormVersionId || versions[0]?.id);
      if (versions.length) showEvaluationStats(courseId, select.val(), results);
    });
  }
  function showEvaluationStats(courseId, versionId, host) {
    host.empty();
    api('/api/evaluation/courses/' + courseId + '/statistics?versionId=' + encodeURIComponent(versionId), 'GET').done(rows => {
      if (!rows.length) $('<p class="muted">').text(t('noRows')).appendTo(host);
      rows.forEach(row => $('<p class="mb-1">').text(name(row.questionAr, row.questionEn) + ' / ' +
        name(row.optionAr, row.optionEn) + ': ' + row.count).appendTo(host));
    });
  }
  function loadCertificateGate(courseId) {
    const host = $('#certificate-gate').empty();
    $('<h4 class="section-title">').text(t('certificateGate')).appendTo(host);
    api('/api/courses/' + courseId + '/certificate-eligibility', 'GET').done(status => {
      $('<p class="fw-bold">').text(t(status.isEligible ? 'eligible' : 'notEligible')).appendTo(host);
      $('<p class="muted">').text(t('attendance') + ': ' + status.attendedLectures + '/' + status.totalLectures).appendTo(host);
      const reasonKeys = ['gateCourseNotEnded', 'gateNoLectures', 'gateAttendance', 'gateExam', 'gateEvaluation', 'gateImpact'];
      status.reasons.forEach(reason => $('<p class="mb-1">').text('• ' + t(reasonKeys[reason])).appendTo(host));
    });
  }
  function searchDirectory(query, facultyId, select) {
    if (query.trim().length < 2) return;
    api('/api/courses/directory-users?query=' + encodeURIComponent(query.trim()) + (facultyId ? '&facultyId=' + encodeURIComponent(facultyId) : ''), 'GET')
      .done(rows => {
        select.empty(); $('<option>').val('').text(t('selectPerson')).appendTo(select);
        rows.forEach(x => $('<option>').val(x.id).text(name(x.nameAr, x.nameEn) + ' (' + x.loginName + ')').data('user', x).appendTo(select));
      });
  }
  $('#open-course-form').on('click', () => { $('#course-edit-id').val(''); $('#course-form')[0].reset();
    $('#course-form-panel').removeClass('d-none'); $('#course-form-panel')[0].scrollIntoView({ behavior: 'smooth' }); });
  $('#cancel-course-form').on('click', () => { $('#course-edit-id').val(''); $('#course-form-panel').addClass('d-none'); });
  $('#course-instructor-search').on('input', function () {
    const facultyId = role('DEAN') && !role('QA') && !role('ADMIN') ? currentAccount?.facultyId : $('#course-faculty').val();
    searchDirectory($(this).val(), facultyId, $('#course-instructor-results'));
  });
  $('#course-instructor-results').on('change', function () {
    instructorChoice = $(this).find(':selected').data('user') || null;
    if (instructorChoice) { $('#course-actual-instructor').val(instructorChoice.nameAr); $('#course-quality-operated').prop('checked', false); }
  });
  $('#course-quality-operated').on('change', function () {
    if ($(this).is(':checked')) { instructorChoice = null; $('#course-instructor-results').val(''); }
  });
  $('#course-form').on('submit', function (e) {
    e.preventDefault(); const start = parseDateTime($('#course-start').val()), end = parseDateTime($('#course-end').val());
    const deadlineText = $('#course-deadline').val().trim(), deadline = deadlineText ? parseDateTime(deadlineText) : null;
    const qualityOperated = $('#course-quality-operated').is(':checked');
    if (!start || !end || (deadlineText && !deadline) ||
        ((role('ADMIN') || role('QA')) && !deadline) || !$('#course-title-ar').val().trim() ||
        !$('#course-objectives-ar').val().trim() || !$('#course-audience-ar').val().trim() ||
        !$('#course-actual-instructor').val().trim() || (!instructorChoice && !qualityOperated)) {
      notice('#course-message', role('DEAN') && !instructorChoice ? t('notEligibleExternal') : t('invalidCourse')); return;
    }
    const data = { titleAr: $('#course-title-ar').val().trim(), titleEn: $('#course-title-en').val().trim() || null,
      trainingTrackId: $('#course-track').val(), objectivesAr: $('#course-objectives-ar').val().trim(),
      objectivesEn: $('#course-objectives-en').val().trim() || null, targetAudienceAr: $('#course-audience-ar').val().trim(),
      targetAudienceEn: $('#course-audience-en').val().trim() || null, deliveryMode: Number($('#course-mode').val()),
      plannedHours: Number($('#course-hours').val()), plannedStartUtc: start, plannedEndUtc: end,
      facultyId: $('#course-faculty').val() || null, instructorUserId: qualityOperated ? null : instructorChoice?.id || null,
      actualInstructorNameAr: $('#course-actual-instructor').val().trim(), actualInstructorNameEn: null,
      qualityOperated, hasExam: $('#course-has-exam').is(':checked'), enrollmentMode: Number($('#course-enrollment').val()),
      evaluationDeadlineUtc: deadline };
    const button = $(this).find('button[type=submit]').prop('disabled', true);
    const editId = $('#course-edit-id').val();
    api(editId ? '/api/courses/' + editId + '/resubmit' : '/api/courses', editId ? 'PUT' : 'POST', data)
      .done(() => { $('#course-form')[0].reset(); $('#course-edit-id').val(''); instructorChoice = null;
      $('#course-form-panel').addClass('d-none'); notice('#course-message', t('courseCreated'), true); loadCourses(); })
      .fail(xhr => notice('#course-message', xhr.responseJSON?.message || t('failed'))).always(() => button.prop('disabled', false));
  });
  $('#course-list').on('click', '.course-view', function () { showCourseDetails($(this).data('id')); })
    .on('click', '.course-enroll', function () {
      api('/api/courses/' + $(this).data('id') + '/enroll', 'POST').done(() => { notice('#course-message', t('enrolled'), true); loadCourses(); })
        .fail(xhr => notice('#course-message', xhr.responseJSON?.message || t('failed')));
    });
  $('#close-course-details').on('click', () => { $('#course-details-panel').addClass('d-none'); currentCourse = null; });
  $('#course-detail-content').on('change', '.evaluation-stats-version', function () {
    showEvaluationStats(currentCourse.course.id, $(this).val(), $('#evaluation-stats .evaluation-stats-results'));
  });
  $('#course-detail-content').on('submit', '#lecture-form', function (e) {
    e.preventDefault(); const nameAr = $(this).find('[name=title]').val().trim(), textDate = $(this).find('[name=date]').val().trim();
    const startsAtUtc = textDate ? parseDateTime(textDate) : null;
    if (!nameAr || (textDate && !startsAtUtc)) { notice('#course-detail-message', t('invalidCourse')); return; }
    api('/api/courses/' + currentCourse.course.id + '/lectures', 'POST', { nameAr, nameEn: null, startsAtUtc })
      .done(() => { showCourseDetails(currentCourse.course.id); loadCourses(); })
      .fail(xhr => notice('#course-detail-message', xhr.responseJSON?.message || t('failed')));
  }).on('submit', '#cancellation-form', function (e) {
    e.preventDefault(); const file = $(this).find('[name=file]')[0].files[0];
    const reason = $(this).find('[name=reason]').val().trim();
    if (!file || !reason) { notice('#course-detail-message', t('invalidCourse')); return; }
    const data = new FormData(); data.append('reasonAr', reason); data.append('file', file);
    multipart('/api/course-cancellations/' + currentCourse.course.id, 'POST', data)
      .done(() => { notice('#course-detail-message', t('saved'), true); loadQualityRequests(); })
      .fail(xhr => notice('#course-detail-message', xhr.responseJSON?.message || t('failed')));
  }).on('click', '.activate-code', function () {
    api('/api/courses/lectures/' + $(this).data('id') + '/activate-code', 'POST').done(result => {
      notice('#course-detail-message', result.attendanceCode + ' · ' + t('codeActive') + ' ' +
        new Date(result.expiresAtUtc).toLocaleTimeString(language === 'ar' ? 'ar-JO' : 'en-GB'), true);
    }).fail(xhr => notice('#course-detail-message', xhr.responseJSON?.message || t('failed')));
  }).on('submit', '.code-form', function (e) {
    e.preventDefault(); const lectureId = $(this).data('id'), code = $(this).find('.code-input').val().trim();
    if (!/^\d{4}$/.test(code)) { notice('#course-detail-message', t('failed')); return; }
    api('/api/courses/lectures/' + lectureId + '/attendance-code', 'POST', { code })
      .done(() => { notice('#course-detail-message', t('codeAccepted'), true); showCourseDetails(currentCourse.course.id); })
      .fail(xhr => notice('#course-detail-message', xhr.responseJSON?.message || t('failed')));
  }).on('change', '.manual-attendance', function () {
    const checkbox = $(this), lectureId = checkbox.data('lecture'), enrollmentId = checkbox.data('enrollment');
    api('/api/courses/lectures/' + lectureId + '/attendance/' + enrollmentId, 'PUT', { isPresent: checkbox.is(':checked') })
      .done(() => showCourseDetails(currentCourse.course.id))
      .fail(xhr => { checkbox.prop('checked', !checkbox.is(':checked')); notice('#course-detail-message', xhr.responseJSON?.message || t('failed')); });
  }).on('click', '.end-course', function () {
    if (!window.confirm(t('confirmEnd'))) return;
    api('/api/courses/' + currentCourse.course.id + '/end', 'POST').done(() => { showCourseDetails(currentCourse.course.id); loadCourses(); })
      .fail(xhr => notice('#course-detail-message', xhr.responseJSON?.message || t('failed')));
  }).on('submit', '#exam-form', function (e) {
    e.preventDefault(); const nameAr = $(this).find('[name=name]').val().trim(); if (!nameAr) return;
    api('/api/courses/' + currentCourse.course.id + '/exam', 'POST', { nameAr, nameEn: null, startsAtUtc: null })
      .done(() => showCourseDetails(currentCourse.course.id))
      .fail(xhr => notice('#course-detail-message', xhr.responseJSON?.message || t('failed')));
  }).on('change', '.exam-outcome', function () {
    const select = $(this), outcome = Number(select.val()); if (!outcome) return;
    api('/api/courses/' + currentCourse.course.id + '/exam/results/' + select.data('enrollment'), 'PUT', { outcome })
      .done(() => showCourseDetails(currentCourse.course.id))
      .fail(xhr => notice('#course-detail-message', xhr.responseJSON?.message || t('failed')));
  }).on('input', '#participant-search', function () { searchDirectory($(this).val(), null, $('#participant-results')); })
    .on('click', '#add-participant', function () {
      const person = $('#participant-results').val(); if (!person) { notice('#course-detail-message', t('selectPerson')); return; }
      api('/api/courses/' + currentCourse.course.id + '/participants', 'POST', { userId: person })
        .done(() => showCourseDetails(currentCourse.course.id))
        .fail(xhr => notice('#course-detail-message', xhr.responseJSON?.message || t('failed')));
    }).on('submit', '#exam-requirement-form', function (event) {
      event.preventDefault();
      const required = $(this).find('[name=required]').val() === 'true';
      api('/api/courses/' + currentCourse.course.id + '/exam-requirement', 'PUT', { required })
        .done(() => { notice('#course-detail-message', t('saved'), true); showCourseDetails(currentCourse.course.id); })
        .fail(xhr => notice('#course-detail-message', xhr.responseJSON?.message || t('failed')));
    }).on('submit', '#course-evaluation-form', function (event) {
      event.preventDefault();
      const answers = $(this).find('.lecture-card').map(function () {
        return { questionId: $(this).data('question'), optionId: $(this).find('input:checked').val() };
      }).get();
      api('/api/evaluation/courses/' + currentCourse.course.id + '/responses', 'POST', { answers })
        .done(() => { notice('#course-detail-message', t('saved'), true); loadEvaluationForCourse(currentCourse.course.id); })
        .fail(xhr => notice('#course-detail-message', xhr.responseJSON?.message || t('failed')));
    }).on('submit', '#impact-upload-form', function (event) {
      event.preventDefault();
      const file = $(this).find('[name=file]')[0].files[0];
      if (!file || file.size > 20 * 1024 * 1024 || !/\.(pdf|docx)$/i.test(file.name)) {
        notice('#course-detail-message', t('impactFile')); return;
      }
      const data = new FormData(); data.append('file', file);
      $.ajax({ url: apiBase + '/api/impact/courses/' + currentCourse.course.id, method: 'POST',
        data, processData: false, contentType: false, xhrFields: { withCredentials: true } })
        .done(() => { notice('#course-detail-message', t('saved'), true); loadMyImpact(currentCourse.course.id); })
        .fail(xhr => notice('#course-detail-message', xhr.responseJSON?.message || t('failed')));
    }).on('click', '.review-impact', function () {
      const approve = $(this).data('approve');
      const noteAr = approve ? null : window.prompt(t('reviewNote'));
      if (!approve && !noteAr?.trim()) return;
      api('/api/impact/' + $(this).data('id') + '/review', 'PUT', { approve, noteAr })
        .done(() => { notice('#course-detail-message', t('saved'), true); loadImpactRoster(currentCourse.course.id); })
        .fail(xhr => notice('#course-detail-message', xhr.responseJSON?.message || t('failed')));
    }).on('submit', '#extend-evaluation-form', function (event) {
      event.preventDefault();
      const deadlineUtc = parseDateTime($(this).find('[name=deadline]').val().trim());
      if (!deadlineUtc) { notice('#course-detail-message', t('invalidCourse')); return; }
      api('/api/courses/' + currentCourse.course.id + '/evaluation-deadline', 'PUT', { deadlineUtc })
        .done(() => { notice('#course-detail-message', t('saved'), true); showCourseDetails(currentCourse.course.id); })
        .fail(xhr => notice('#course-detail-message', xhr.responseJSON?.message || t('failed')));
    }).on('click', '.review-proposal', function () {
      const noteAr = $('#proposal-review-note').val().trim();
      if (!noteAr) { notice('#course-detail-message', t('proposalReviewNote')); return; }
      api('/api/courses/' + currentCourse.course.id + '/review', 'POST',
        { decision: $(this).data('decision'), noteAr })
        .done(() => { showCourseDetails(currentCourse.course.id); loadCourses(); })
        .fail(xhr => notice('#course-detail-message', xhr.responseJSON?.message || t('failed')));
    }).on('click', '.edit-proposal', function () {
      const detail = currentCourse, course = detail.course;
      $('#course-edit-id').val(course.id);
      $('#course-title-ar').val(course.titleAr); $('#course-title-en').val(course.titleEn || '');
      $('#course-track').val(course.trainingTrackId);
      $('#course-objectives-ar').val(detail.objectivesAr); $('#course-objectives-en').val(detail.objectivesEn || '');
      $('#course-audience-ar').val(detail.targetAudienceAr); $('#course-audience-en').val(detail.targetAudienceEn || '');
      $('#course-mode').val(String(detail.deliveryMode)); $('#course-hours').val(detail.plannedHours);
      $('#course-start').val(localDateTime(detail.plannedStartUtc));
      $('#course-end').val(localDateTime(detail.plannedEndUtc));
      $('#course-faculty').val(course.facultyId || ''); $('#course-enrollment').val(String(course.enrollmentMode));
      $('#course-actual-instructor').val(course.actualInstructorNameAr);
      instructorChoice = course.instructorUserId ? { id: course.instructorUserId } : null;
      $('#course-has-exam').prop('checked', course.hasExam);
      $('#course-form-panel').removeClass('d-none')[0].scrollIntoView({ behavior: 'smooth' });
    }).on('click', '.approve-course', function () {
      const deadline = parseDateTime($('#approval-deadline').val().trim());
      if (!deadline) { notice('#course-detail-message', t('invalidCourse')); return; }
      api('/api/courses/' + currentCourse.course.id + '/approve', 'POST', {
        evaluationDeadlineUtc: deadline, qualityOperated: $('#approval-quality').is(':checked'),
        instructorUserId: currentCourse.course.instructorUserId
      }).done(() => { showCourseDetails(currentCourse.course.id); loadCourses(); })
        .fail(xhr => notice('#course-detail-message', xhr.responseJSON?.message || t('failed')));
    });
  function verifyCertificate(reference) {
    const host = $('#verification-result').empty();
    api('/api/certificates/verify?cert=' + encodeURIComponent(reference), 'GET').done(value => {
      $('<div class="lecture-card">').appendTo(host)
        .append($('<h4>').text(t(value.status === 0 ? 'certificateValid' : 'certificateRevoked')))
        .append($('<p>').text(t('recipient') + ': ' + name(value.recipientNameAr, value.recipientNameEn)))
        .append($('<p>').text(t('courseName') + ': ' + name(value.courseTitleAr, value.courseTitleEn)))
        .append($('<p>').text(t('instructor') + ': ' + name(value.instructorNameAr, value.instructorNameEn)))
        .append($('<p>').text(t('issuedAt') + ': ' + new Date(value.issuedAtUtc).toLocaleDateString(language === 'ar' ? 'ar-JO' : 'en-GB')));
      if (value.statusReason) $('<p class="text-danger">').text(value.statusReason).appendTo(host.find('.lecture-card'));
    }).fail(() => $('<div class="alert alert-warning">').text(t('certificateNotFound')).appendTo(host));
  }
  $('#open-verification').on('click', () => show('verification'));
  $('#back-to-login').on('click', () => show('login'));
  $('#verification-form').on('submit', function (event) {
    event.preventDefault(); const reference = $('#verification-reference').val().trim();
    if (reference) verifyCertificate(reference);
  });
  translate();
  $.getJSON('/config/api.json').done(c => {
    apiBase = c.baseUrl.replace(/\/$/, '');
    api('/api/settings/hints', 'GET').done(rows => { hints = rows; renderHints(); });
    api('/api/settings/password-policy', 'GET').done(p => {
      $('#new-password').attr('minlength', p.minimumLength).attr('maxlength', p.maximumLength);
      $('#change-view .form-text').text(language === 'ar' ?
        'من ' + p.minimumLength + ' إلى ' + p.maximumLength + ' حرفًا.' :
        'Between ' + p.minimumLength + ' and ' + p.maximumLength + ' characters.');
    });
    const certificateReference = new URLSearchParams(window.location.search).get('cert');
    if (certificateReference) {
      show('verification'); $('#verification-reference').val(certificateReference);
      verifyCertificate(certificateReference);
    } else loadAccount();
  })
    .fail(() => { show('login'); notice('#status-message', t('connectionError')); });
})();
