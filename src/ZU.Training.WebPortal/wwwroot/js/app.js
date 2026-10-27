(function () {
  'use strict';
  let apiBase = '', language = 'ar', rememberMe = false, signedIn = false, hints = [], templates = [];
  let currentAccount = null, courses = [], tracks = [], faculties = [], currentCourse = null, instructorChoice = null;
  let adminRoles = [], adminUsers = [], selectedUser = null;
  let adminCourseRows = [];
  let qualityNeeds = [], cancellationRequests = [];
  let historicalParticipants = [];

  const labels = {
    ar: {
      brand: 'جامعة الزرقاء', platform: 'منصة التدريب والتطوير المهني لأعضاء هيئة التدريس', eyebrow: 'منصة الجامعة الرسمية المعتمدة',
      heading: 'تطوير أكاديمي ومهني ريادي مستدام', intro: 'منظومة رقمية متكاملة لإدارة الدورات التدريبية، وتوثيق الحضور الذكي، وتقييم الأثر التدريبي، وإصدار الشهادات الرقمية المعتمدة بكود QR فوري.',
      point1: 'دورات معتمدة متخصصة', point2: 'حضور رقمي ذكي ولحظي', point3: 'شهادات مشفرة برمز QR', point4: 'قياس أثر التدريب المهني',
      loginTitle: 'تسجيل الدخول', loginHelp: 'أدخل الرقم الوظيفي وكلمة المرور الخاصة بك.', employeeNumber: 'الرقم الوظيفي / اسم الدخول', password: 'كلمة المرور',
      remember: 'تذكرني', login: 'دخول', changeTitle: 'تعيين كلمة مرور جديدة',
      changeHelp: 'لأمان حسابك، غيّر كلمة المرور الأولية قبل البدء في استخدام المنصة.', oldPassword: 'كلمة المرور الأولية الحالية',
      newPassword: 'كلمة المرور الجديدة', confirmPassword: 'تأكيد كلمة المرور الجديدة', passwordRule: 'يجب أن تطابق سياسة كلمة المرور المعتمدة بالجامعة.',
      savePassword: 'حفظ والمتابعة إلى المنصة', welcome: 'مرحبًا بك', workspace: 'مساحة العمل', logout: 'تسجيل الخروج', roles: 'الأدوار: ',
      role_ADMIN: 'مسؤول النظام', role_SYSTEM_ADMIN: 'مسؤول النظام', role_QA: 'ضمان الجودة والاعتماد', role_QUALITY_DIRECTOR: 'مدير الجودة',
      role_QUALITY_STAFF: 'كادر الجودة', role_DEAN: 'عميد الكلية', role_INST: 'المحاضر', role_INSTRUCTOR: 'المحاضر',
      role_EXTERNAL_INSTRUCTOR: 'محاضر خارجي', role_MEMBER: 'عضو هيئة تدريس',
      forgotPassword: 'نسيت كلمة المرور؟', forgotTitle: 'استعادة كلمة المرور', forgotHelp: 'أدخل رقمك الوظيفي وبريدك الإلكتروني الجامعي المسجل لتلقي بيانات الدخول.',
      sendPassword: 'إرسال كلمة المرور', backToLogin: 'العودة لتسجيل الدخول', passwordSent: 'تم إرسال بيانات الدخول إلى بريدك الإلكتروني بنجاح.',
      navTrainee: 'مساحة المتدرب', navInstructor: 'دوراتي كمحاضر', navQuality: 'إدارة الجودة والعمادة',
      navAnalytics: 'الإحصائيات والتقارير', navVault: 'المستودع المركزي للوثائق', navAdmin: 'إدارة النظام (الأدمن)',
      kpiEnrolled: 'الدورات المسجل بها', kpiCompleted: 'الدورات المنجزة', kpiHours: 'الساعات المكتسبة', kpiCertificates: 'الشهادات المعتمدة',
      filterAll: 'جميع الدورات', filterEnrolled: 'دوراتي المسجل بها', filterAvailable: 'متاحة للتسجيل الذاتي',
      myCertificatesTitle: 'سجل شهاداتي الرقمية المعتمدة', myCertificatesHelp: 'شهاداتك الرسمية الصادرة من جامعة الزرقاء مع رمز التحقق QR ورابط الفحص المباشر وإمكانية التحميل الفوري بصيغة PDF.',
      instructorCoursesTitle: 'دوراتي كمحاضر / مدرب معتمد', instructorCoursesHelp: 'بيئة عمل المحاضر المستقلة: إدارة الجلسات، كود الحضور اللحظي، رصد الحضور اليدوي، الامتحانات، واعتماد الأثر التدريبي.',
      instructorEmptyMsg: 'لا توجد دورات مسندة لك كمدرب حالياً، ستظهر هنا تلقائياً فور اعتماد تكليفك من الكلية والجودة.',
      proposalsTab: 'مقترحات الدورات', proposalsTitle: 'مقترحات البرامج التدريبية', proposalsHelp: 'متابعة مقترحات العمداء والاعتماد الرسمي وتحديد مهل التقييم.',
      historicalTab: 'توثيق الدورات التاريخية', historicalTitle: 'توثيق الدورات التاريخية السابقة وإصدار شهاداتها',
      historicalHelp: 'توثيق الدورات السابقة المنعقدة قبل إطلاق النظام وإصدار الشهادات فورياً للمشاركين المستوفين لكافة الشروط المعتمدة (حضور 75% فما فوق).',
      saveHistorical: 'توثيق الدورة وإصدار الشهادات آلياً', historicalSaved: 'تم توثيق الدورة وإصدار الشهادات للمشاركين المستحقين بنجاح.',
      analyticsTitle: 'المؤشرات والإحصائيات الشاملة لجامعة الزرقاء', analyticsHelp: 'لوحة المؤشرات الرقمية المركزية لضمان الجودة والاعتماد الأكاديمي على مستوى الجامعة وكلياتها ومحاورها.',
      vaultTitle: 'المستودع المركزي للوثائق والكتب الرسمية', vaultHelp: 'أرشيف رقمي لكافة الكتب الرسمية، كتب التكليف، كتب الإلغاء، وخطط الأثر التدريبي بصيغتي PDF و DOCX.',
      auditLogsTab: 'سجل العمليات (Audit Logs)', auditLogsHelp: 'سجل زمني لكافة الأحداث والعمليات الإدارية والتشغيلية في النظام دون كشف كلمات المرور.',
      dispatchEmails: 'إرسال بيانات الدخول الأولية بالبريد', dispatchSuccess: 'تم إرسال رسائل البريد الإلكتروني الأولية بنجاح.',
      hintsTab: 'تلميحات الحقول', policyTab: 'سياسة كلمة المرور', templatesTab: 'قالب الشهادة',
      hintsTitle: 'تلميحات الحقول الديناميكية', hintsHelp: 'يظهر النص عند لمس الحقل أو التركيز عليه، وتسري التعديلات فورياً لكافة الشاشات.',
      screenKey: 'مفتاح الشاشة', fieldKey: 'مفتاح الحقل', hintAr: 'التلميح بالعربية', hintEn: 'التلميح بالإنجليزية (اختياري)',
      saveHint: 'حفظ التلميح', clear: 'إفراغ', actions: 'إجراءات', edit: 'تعديل', remove: 'حذف',
      policyTitle: 'سياسة كلمات المرور المركزية', policyHelp: 'تطبق القواعد الجديدة عند تغيير كلمة المرور القادم، ولا تلزم الكلمات الحالية بالتغيير.',
      minimumLength: 'الحد الأدنى للأحرف', maximumLength: 'الحد الأعلى للأحرف', allowSpaces: 'السماح بالمسافات',
      requireMix: 'اشتراط حروف وأرقام', blockCommon: 'منع كلمات المرور الشائعة', savePolicy: 'حفظ السياسة',
      templatesTitle: 'قالب الشهادة الرقمية المعتمد', templatesHelp: 'ارفع قالب PDF الثابت في أي وقت. يحتفظ النظام بنسخ القوالب المستخدمة سابقاً.',
      templateNameAr: 'اسم القالب بالعربية', templateNameEn: 'الاسم بالإنجليزية (اختياري)', pdfFile: 'ملف PDF',
      uploadTemplate: 'رفع القالب', version: 'النسخة', uploadDate: 'تاريخ الرفع', download: 'تنزيل',
      otherComing: 'وحدات الدورات قيد التطوير؛ لا توجد إجراءات متاحة لهذا الدور بعد.',
      missingLogin: 'أدخل الرقم الوظيفي وكلمة المرور.', invalidLogin: 'بيانات الدخول غير صحيحة أو الحساب غير متاح.',
      passwordMismatch: 'تأكيد كلمة المرور غير مطابق.', changeFailed: 'تعذر تغيير كلمة المرور. تحقق من الكلمة الحالية والسياسة.',
      connectionError: 'تعذر الاتصال بالخادم.', saved: 'تم الحفظ بنجاح.', deleted: 'تم الحذف.', uploaded: 'تم رفع القالب بنجاح.',
      failed: 'تعذر تنفيذ العملية.', invalidHint: 'املأ مفتاح الشاشة والحقل والتلميح العربي.',
      invalidPolicy: 'تحقق من الحدود الرقمية لسياسة كلمة المرور.', invalidPdf: 'اختر ملف PDF لا يزيد عن 20 ميغابايت.',
      confirmDelete: 'هل تريد حذف هذا التلميح؟', noRows: 'لا توجد بيانات مسجلة حالياً.',
      coursesTitle: 'الدورات التدريبية المتاحة', coursesHelp: 'استعرض دورات كليتك والدورات العامة وسجل ذاتياً في الدورات المفتوحة.', newCourse: 'اقتراح دورة جديدة',
      courseFormTitle: 'اقتراح برنامج تدريبي جديد', courseFormHelp: 'الحقول العربية إلزامية، والإنجليزية اختيارية. يختار العميد محاضراً من كليته أو خبيراً خارجياً.',
      courseTitleAr: 'اسم الدورة بالعربية', courseTitleEn: 'اسم الدورة بالإنجليزية (اختياري)', trainingTrack: 'المحور التدريبي',
      deliveryMode: 'نمط الانعقاد', inPerson: 'وجاهي (بالحرم الجامعي)', online: 'عن بعد (Microsoft Teams)', hybrid: 'مدمج (وجاهي وعن بعد)',
      objectivesAr: 'الأهداف ونواتج التعلم بالعربية', objectivesEn: 'الأهداف بالإنجليزية (اختياري)',
      audienceAr: 'الفئة المستهدفة بالعربية', audienceEn: 'الفئة المستهدفة بالإنجليزية (اختياري)', plannedHours: 'عدد الساعات المعتمدة',
      startDate: 'موعد البداية (YYYY-MM-DD HH:mm)', endDate: 'موعد النهاية (YYYY-MM-DD HH:mm)',
      faculty: 'الكلية (اتركه فارغاً للدورات العامة لجميع الكليات)', enrollmentMode: 'طريقة التحاق المتدربين',
      selfRegistration: 'تسجيل ذاتي لأعضاء الكلية/الجامعة', qualitySelection: 'اختيار الجودة للمشاركين بالاسم',
      actualInstructor: 'اسم المحاضر الفعلي بالعربية', instructorSearch: 'البحث عن حساب المحاضر من الكلية',
      manualInstructor: 'دون حساب محاضر (محاضر خارجي)', qualityOperated: 'تدير الجودة الدورة القصيرة نيابة عن المحاضر',
      hasExam: 'الدورة مشروطة باجتياز امتحان خارجي', evaluationDeadline: 'نهاية فترة التقييم (تحدده الجودة)',
      saveCourse: 'حفظ وإرسال المقترح', cancel: 'إلغاء', close: 'إغلاق', view: 'عرض التفاصيل',
      enroll: 'تسجيل فوري', registered: 'مسجل بالدورة', approved: 'معتمدة رسميًا', submitted: 'بانتظار قرار الجودة',
      inProgress: 'جارية الانعقاد', ended: 'منتهية', cancelled: 'ملغاة رسميًا', noCourses: 'لا توجد دورات متاحة لحسابك حالياً.',
      invalidCourse: 'أكمل الحقول العربية والمواعيد بدقة، واختر محاضراً معتمداً أو تشغيل الجودة.',
      courseCreated: 'تم حفظ وإرسال المقترح بنجاح.', enrolled: 'تم تسجيلك في الدورة بنجاح.',
      lectureTitle: 'عنوان المحاضرة / الجلسة بالعربية', lectureDate: 'موعد المحاضرة (اختياري)',
      addLecture: 'إضافة جلسة', lectures: 'الجلسات التدريبية', activateCode: 'تفعيل كود الحضور 5 دقائق',
      attendanceCode: 'كود الحضور (4 أرقام)', submitCode: 'تأكيد الحضور', attendance: 'حضور الجلسة',
      participants: 'قائمة المشاركين', addParticipant: 'إضافة مشارك للدورة', searchParticipant: 'ابحث عن العضو بالاسم أو الرقم',
      endCourse: 'إنهاء الدورة رسميًا', confirmEnd: 'هل تؤكد إنهاء الدورة؟ سيتم إغلاق إضافة الجلسات وبدء استحقاق التقييم والشهادات.',
      exam: 'الامتحان', examName: 'اسم الامتحان بالعربية', createExam: 'إنشاء الامتحان',
      notRecorded: 'لم تُرصد النتيجة', passed: 'ناجح', failed: 'راسب', approveCourse: 'اعتماد الدورة رسميًا',
      selectPerson: 'اختر عضواً من نتائج البحث', codeActive: 'كود الحضور فعال حتى الساعة', codeAccepted: 'تم توثيق حضورك بنجاح.',
      noLectures: 'لم يتم إنشاء جلسات لهذه الدورة بعد.', notEligibleExternal: 'مقترح المحاضر الخارجي يحتاج مسار الكتاب الرسمي والسيرة الذاتية.',
      usersTab: 'المستخدمون', rolePermissionsTab: 'صلاحيات الأدوار المركزية', usersTitle: 'إدارة المستخدمين والحسابات',
      usersHelp: 'ابحث بالاسم أو رقم الدخول، واعرض الصلاحيات والكلمات المشفرة، وأرسل بيانات الدخول الأولية.',
      userSearch: 'الاسم أو الرقم الوظيفي', search: 'بحث', newUser: 'إنشاء حساب جديد', accountType: 'نوع الحساب',
      employee: 'عضو هيئة تدريس / موظف', externalTrainer: 'محاضر خارجي (برقم 4 أرقام)', externalLogin: 'رقم الدخول (4 أرقام فريدة)',
      nameAr: 'الاسم بالعربية', nameEn: 'الاسم بالإنجليزية (اختياري)', email: 'البريد الإلكتروني الجامعي',
      externalPassword: 'كلمة المرور المحددة للمحاضر الخارجي', rolesLabel: 'الأدوار الممنوحة', saveUser: 'حفظ الحساب',
      showPasswords: 'عرض كلمات المرور بعد فك التشفير', disableUser: 'تعطيل الحساب', restoreUser: 'استعادة الحساب',
      allowAccess: 'تجاوز حالة العمل الجامعية', removeAccessOverride: 'إزالة تجاوز العمل', changePassword: 'تعيين كلمة المرور',
      saveRoles: 'حفظ الأدوار', userPermissions: 'استثناءات صلاحيات المستخدم الفردية',
      userPermissionsHelp: 'لكل صلاحية: اتباع الدور أو سماح أو منع، مع عرض النتيجة الفعلية وسجل التدقيق.',
      sharedPassword: 'كلمة المرور الأولية المشتركة', sharedHelp: 'تُقبل حصراً من حسابات الموظفين التي لم تكمل أول دخول بعد.',
      showShared: 'عرض الكلمة الحالية', newShared: 'كلمة مشتركة جديدة', saveShared: 'حفظ الكلمة المشتركة',
      rolePermissionsTitle: 'صلاحيات الأدوار المركزية', rolePermissionsHelp: 'تغيير الصلاحية الافتراضية ينعكس على كافة مستخدمي الدور ما لم يكن لديهم استثناء فردي.',
      inherited: 'اتباع الدور', allow: 'سماح', deny: 'منع', effective: 'النتيجة الفعلية', yes: 'مسموح', no: 'ممنوع',
      initialPassword: 'كلمة المرور الأولية', currentPassword: 'كلمة المرور الحالية', confirmDisable: 'هل تؤكد تعطيل هذا الحساب؟',
      userCreated: 'تم إنشاء الحساب بنجاح. رقم الدخول:', invalidUser: 'تحقق من صحة بيانات الحساب والأدوار المحددة.', noUsers: 'لا يوجد مستخدمون مطابقون للبحث.',
      universityTab: 'بيانات الجامعة', universityTitle: 'مزامنة بيانات الجامعة المركزية',
      universityHelp: 'استيراد وتحديث الكليات والأقسام والموظفين إلى قاعدة المنصة، مع الحفاظ على كلمات المرور والأدوار المحلية.',
      refreshUniversity: 'بدء تحديث بيانات الجامعة', syncRuns: 'سجل عمليات التحديث',
      missingEmployees: 'موظفون اختفوا من آخر استجابة ناجحة (للمراجعة الإدارية)', syncComplete: 'اكتمل التحديث بنجاح',
      syncIncomplete: 'لم يكتمل التحديث', added: 'مضاف', updated: 'محدّث', missing: 'مفقود',
      needsTab: 'الاحتياجات التدريبية', cancellationsTab: 'طلبات إلغاء الدورات', needsTitle: 'الاحتياجات التدريبية',
      needsHelp: 'يرفع العميد الاحتياج وكتابه الرسمي إلى الجودة فقط.', needTitleAr: 'عنوان الاحتياج بالعربية',
      needTitleEn: 'العنوان بالإنجليزية (اختياري)', needDescriptionAr: 'وصف الاحتياج ومبرراته بالعربية',
      needDescriptionEn: 'الوصف بالإنجليزية (اختياري)', officialLetter: 'الكتاب الرسمي المعتمد (PDF أو DOCX)',
      submitNeed: 'رفع الاحتياج الرسمي للجودة', cancellationsTitle: 'طلبات إلغاء الدورات المعتمدة',
      cancellationsHelp: 'لا تُلغى الدورة المعتمدة إلا بعد كتاب رسمي موقع وقرار الجودة. تلغى شهاداتها وتظهر ملغاة عند التحقق.',
      returnRevision: 'إعادة للتعديل', reject: 'رفض', resubmit: 'إعادة إرسال',
      requestCancellation: 'طلب إلغاء الدورة', cancellationReason: 'سبب الإلغاء بالعربية',
      reviewNote: 'ملاحظة الجودة أو سبب القرار', officialDocument: 'الكتاب الرسمي',
      revisionRequested: 'أعيد للتعديل', rejected: 'مرفوض', needsEmpty: 'لا توجد طلبات مسجلة حالياً.',
      evaluationTab: 'نموذج التقييم الموحد لجامعة الزرقاء', evaluationAdminHelp: 'نموذج مشترك لجميع الدورات. عند النشر تُحفظ نسخة جديدة وتسري على الدورات المفتوحة.',
      addQuestion: 'إضافة سؤال جديد', addOption: 'إضافة خيار', publishEvaluation: 'نشر نسخة جديدة للتقييم',
      questionAr: 'السؤال بالعربية', questionEn: 'السؤال بالإنجليزية (اختياري)', optionAr: 'الخيار بالعربية',
      optionEn: 'الخيار بالإنجليزية (اختياري)', evaluateCourse: 'تقييم الدورة', submitEvaluation: 'إرسال التقييم الرسمي',
      evaluationSubmitted: 'تم تقديم استبيان تقييم هذه الدورة بنجاح.', impactTitle: 'خطة قياس الأثر التدريبي',
      impactFile: 'ملف الأثر التدريبي (PDF أو DOCX)', uploadImpact: 'رفع ملف الأثر', impactPending: 'بانتظار مراجعة المدرب',
      impactReturned: 'أعيد للتعديل من قِبل المدرب', impactApproved: 'معتمد رسمياً من المدرب', approveImpact: 'اعتماد خطة الأثر',
      returnImpact: 'إعادة للتعديل', evaluationStats: 'إحصاءات استجابات التقييم', extendDeadline: 'تمديد مهلة التقييم',
      examRequirement: 'شرط الامتحان الخارجي', examRequired: 'مشروطة باجتياز امتحان', examNotRequired: 'بلا شرط امتحان', changeExamRequirement: 'تعديل شرط الامتحان',
      certificateGate: 'شروط استحقاق الشهادة الرقمية المعتمدة', eligible: 'تهانينا! لقد استوفيت كافة متطلبات الشهادة.', notEligible: 'لم تستوفِ بعد كافة شروط استحقاق الشهادة.',
      gateCourseNotEnded: 'الدورة لم تنتهِ رسميًا بعد', gateNoLectures: 'لم تُعقد جلسات تدريبية بعد', gateAttendance: 'نسبة الحضور أقل من 75%',
      gateExam: 'لم يثبت اجتياز الامتحان بنجاح', gateEvaluation: 'لم يتم استكمال استبيان التقييم', gateImpact: 'خطة قياس الأثر التدريبي غير معتمدة',
      verifyCertificate: 'بوابة التحقق من الشهادات الرقمية', verifyHelp: 'أدخل الرقم المرجعي المطبوع على الشهادة للتأكد من صحتها واعتمادها من جامعة الزرقاء.', certificateReference: 'الرقم المرجعي للشهادة', verify: 'تحقق الآن',
      certificateValid: 'شهادة رقمية رسمية معتمدة وصالحة', certificateRevoked: 'الشهادة ملغاة رسميًا من جامعة الزرقاء', certificateNotFound: 'لم يُعثر على شهادة مسجلة بهذا الرقم المرجعي',
      recipient: 'صاحب الشهادة', courseName: 'اسم البرنامج التدريبي', instructor: 'المدرب / المحاضر', issuedAt: 'تاريخ الإصدار والاعتماد',
      reviewProposal: 'قرار الجودة بشأن المقترح', resubmitProposal: 'تعديل المقترح وإعادة إرساله',
      proposalReviewNote: 'سبب ومبررات قرار الجودة بالعربية', proposalUpdated: 'تم إعادة إرسال المقترح بنجاح.',
      adminCoursesTab: 'إدارة الدورات والحذف المنطقي', adminCoursesHelp: 'صلاحية التعطيل المنطقي لأي دورة مع تحديد مصير شهاداتها، وإمكانية الاستعادة مع إعادة تقييم الاستحقاق.',
      disableCourse: 'تعطيل الدورة', restoreCourse: 'استعادة الدورة', hideCertificates: 'إخفاء الشهادات من بوابة التحقق',
      keepCertificates: 'إبقاء الشهادات ظاهرة', courseDeleteWarning: 'هل تؤكد تعطيل الدورة؟ ستختفي من قوائم الدورات النشطة.',
      courseActive: 'نشطة', courseDisabled: 'معطلة منطقيًا', certificates: 'الشهادات المعتمدة',
      downloadCertificate: 'تحميل الشهادة المعتمدة (PDF)', directVerify: 'التحقق المباشر'
    },
    en: {
      brand: 'Zarqa University', platform: 'Faculty Professional Training & Development Platform', eyebrow: 'Official Accredited University Platform',
      heading: 'Leading & Sustainable Academic Development', intro: 'An integrated digital ecosystem for managing training courses, smart attendance tracking, impact assessment, and issuing verified digital certificates with instant QR verification.',
      point1: 'Accredited Specialized Courses', point2: 'Real-time Smart Attendance', point3: 'Instant QR Verified Certificates', point4: 'Training Impact Measurement',
      loginTitle: 'Sign In', loginHelp: 'Enter your employee number / login ID and password.', employeeNumber: 'Employee Number / Login ID', password: 'Password',
      remember: 'Remember me', login: 'Sign In', changeTitle: 'Set New Password',
      changeHelp: 'For security, please change your initial temporary password before accessing the platform.', oldPassword: 'Current Initial Password',
      newPassword: 'New Password', confirmPassword: 'Confirm New Password', passwordRule: 'Must comply with university password policy.',
      savePassword: 'Save and Continue to Platform', welcome: 'Welcome', workspace: 'Workspace', logout: 'Sign Out', roles: 'Roles: ',
      role_ADMIN: 'System Admin', role_SYSTEM_ADMIN: 'System Admin', role_QA: 'Quality Assurance', role_QUALITY_DIRECTOR: 'Quality Director',
      role_QUALITY_STAFF: 'Quality Staff', role_DEAN: 'Faculty Dean', role_INST: 'Instructor', role_INSTRUCTOR: 'Instructor',
      role_EXTERNAL_INSTRUCTOR: 'External Instructor', role_MEMBER: 'Faculty Member',
      forgotPassword: 'Forgot password?', forgotTitle: 'Password Recovery', forgotHelp: 'Enter your employee number and registered university email to receive login details.',
      sendPassword: 'Send Password', backToLogin: 'Back to Sign In', passwordSent: 'Login credentials have been sent to your university email.',
      navTrainee: 'Trainee Workspace', navInstructor: 'My Instructor Courses', navQuality: 'Quality & Deanship',
      navAnalytics: 'Analytics & Reports', navVault: 'Document Vault', navAdmin: 'Administration',
      kpiEnrolled: 'Enrolled Courses', kpiCompleted: 'Completed Courses', kpiHours: 'Earned Hours', kpiCertificates: 'Certified Certificates',
      filterAll: 'All Courses', filterEnrolled: 'My Enrolled Courses', filterAvailable: 'Available for Self-Enrollment',
      myCertificatesTitle: 'My Verified Digital Certificates', myCertificatesHelp: 'Official certificates issued by Zarqa University with QR verification code, direct validation link, and instant PDF download.',
      instructorCoursesTitle: 'My Courses as Certified Instructor', instructorCoursesHelp: 'Independent instructor workspace: manage sessions, live attendance code, manual correction, exams, and impact assessment approval.',
      instructorEmptyMsg: 'No courses are currently assigned to you as an instructor. They will appear here once officially approved.',
      proposalsTab: 'Course Proposals', proposalsTitle: 'Training Program Proposals', proposalsHelp: 'Review proposals submitted by deans, official approvals, and evaluation deadlines.',
      historicalTab: 'Historical Courses', historicalTitle: 'Backfill Historical Courses & Issue Certificates',
      historicalHelp: 'Record past courses held prior to platform launch and issue accredited digital certificates to eligible attendees (75%+ attendance).',
      saveHistorical: 'Record Course & Issue Certificates', historicalSaved: 'Course recorded and certificates issued successfully.',
      analyticsTitle: 'Comprehensive KPIs & Analytics for Zarqa University', analyticsHelp: 'Centralized quality assurance analytics and institutional accreditation reporting.',
      vaultTitle: 'Central Document & Official Letters Vault', vaultHelp: 'Digital archive of all official letters, assignment memos, cancellation records, and impact plans (PDF & DOCX).',
      auditLogsTab: 'System Audit Logs', auditLogsHelp: 'Chronological timeline of all administrative and operational activities with no password exposure.',
      dispatchEmails: 'Batch Email Initial Credentials', dispatchSuccess: 'Initial credentials dispatched successfully.',
      hintsTab: 'Field Hints', policyTab: 'Password Policy', templatesTab: 'Certificate Template',
      hintsTitle: 'Dynamic Field Hints', hintsHelp: 'Contextual hints displayed upon field focus. Changes reflect immediately across all screens.',
      screenKey: 'Screen Key', fieldKey: 'Field Key', hintAr: 'Arabic Hint', hintEn: 'English Hint (Optional)',
      saveHint: 'Save Hint', clear: 'Clear', actions: 'Actions', edit: 'Edit', remove: 'Delete',
      policyTitle: 'Central Password Policy', policyHelp: 'New rules apply on next password reset and do not force existing passwords to change.',
      minimumLength: 'Minimum Length', maximumLength: 'Maximum Length', allowSpaces: 'Allow Spaces',
      requireMix: 'Require Mixed Character Classes', blockCommon: 'Block Common Passwords', savePolicy: 'Save Policy',
      templatesTitle: 'Accredited Certificate Template', templatesHelp: 'Upload fixed background PDF anytime. Previous template versions are archived.',
      templateNameAr: 'Arabic Template Name', templateNameEn: 'English Name (Optional)', pdfFile: 'PDF File',
      uploadTemplate: 'Upload Template', version: 'Version', uploadDate: 'Upload Date', download: 'Download',
      otherComing: 'Course modules are in development; no actions are available for this role yet.',
      missingLogin: 'Enter your employee number and password.', invalidLogin: 'Invalid credentials or inactive account.',
      passwordMismatch: 'Password confirmation does not match.', changeFailed: 'Could not change password. Check current password and policy.',
      connectionError: 'Cannot connect to server.', saved: 'Saved successfully.', deleted: 'Deleted.', uploaded: 'Template uploaded successfully.',
      failed: 'Operation failed.', invalidHint: 'Provide screen key, field key, and Arabic hint.',
      invalidPolicy: 'Check numeric limits for password policy.', invalidPdf: 'Select a valid PDF file under 20MB.',
      confirmDelete: 'Are you sure you want to delete this hint?', noRows: 'No records found.',
      coursesTitle: 'Available Training Courses', coursesHelp: 'Browse faculty and general university courses and self-enroll in open programs.', newCourse: 'Propose New Course',
      courseFormTitle: 'Propose New Training Program', courseFormHelp: 'Arabic fields are mandatory; English is optional.',
      courseTitleAr: 'Arabic Course Title', courseTitleEn: 'English Course Title (Optional)', trainingTrack: 'Training Track',
      deliveryMode: 'Delivery Mode', inPerson: 'In-Person (Campus)', online: 'Online (Microsoft Teams)', hybrid: 'Hybrid (Campus & Online)',
      objectivesAr: 'Objectives and Outcomes (Arabic)', objectivesEn: 'Objectives (English - Optional)',
      audienceAr: 'Target Audience (Arabic)', audienceEn: 'Target Audience (English - Optional)', plannedHours: 'Accredited Hours',
      startDate: 'Start Time (YYYY-MM-DD HH:mm)', endDate: 'End Time (YYYY-MM-DD HH:mm)',
      faculty: 'Faculty (Leave empty for university-wide courses)', enrollmentMode: 'Enrollment Mode',
      selfRegistration: 'Self-Registration for Faculty Members', qualitySelection: 'Quality Assigns Participants by Name',
      actualInstructor: 'Actual Instructor Name in Arabic', instructorSearch: 'Search Faculty Instructor Account',
      manualInstructor: 'External Instructor (Without System Account)', qualityOperated: 'Quality operates short course directly',
      hasExam: 'Course requires passing external exam', evaluationDeadline: 'Evaluation Deadline (Set by Quality)',
      saveCourse: 'Save & Submit Proposal', cancel: 'Cancel', close: 'Close', view: 'View Details',
      enroll: 'Enroll Now', registered: 'Enrolled', approved: 'Approved', submitted: 'Awaiting Quality Decision',
      inProgress: 'In Progress', ended: 'Ended', cancelled: 'Cancelled', noCourses: 'No courses currently visible for your account.',
      invalidCourse: 'Please fill Arabic fields and dates properly, and select an instructor or Quality operation.',
      courseCreated: 'Course proposal saved and submitted successfully.', enrolled: 'You have been enrolled successfully.',
      lectureTitle: 'Session / Lecture Title in Arabic', lectureDate: 'Session Time (Optional)',
      addLecture: 'Add Session', lectures: 'Training Sessions', activateCode: 'Activate 5-Min Attendance Code',
      attendanceCode: 'Attendance Code (4 Digits)', submitCode: 'Submit Code', attendance: 'Attendance',
      participants: 'Participants Roster', addParticipant: 'Add Participant', searchParticipant: 'Search member by name or number',
      endCourse: 'End Course Officially', confirmEnd: 'Confirm ending this course? Adding lectures will close and certificate eligibility will evaluate.',
      exam: 'Exam', examName: 'Arabic Exam Name', createExam: 'Create Exam',
      notRecorded: 'Not Recorded', passed: 'Passed', failed: 'Failed', approveCourse: 'Officially Approve Course',
      selectPerson: 'Select a member from results', codeActive: 'Code is active until', codeAccepted: 'Attendance recorded successfully.',
      noLectures: 'No sessions recorded yet.', notEligibleExternal: 'External instructor proposals require official letter and CV workflow.',
      usersTab: 'Users', rolePermissionsTab: 'Central Role Permissions', usersTitle: 'User & Account Management',
      usersHelp: 'Search by name or ID, view decrypted passwords and permissions, and dispatch initial credentials.',
      userSearch: 'Name or Employee Number', search: 'Search', newUser: 'Create New Account', accountType: 'Account Type',
      employee: 'Faculty Member / Employee', externalTrainer: 'External Instructor (4-digit ID)', externalLogin: 'Login ID (Unique 4 Digits)',
      nameAr: 'Arabic Name', nameEn: 'English Name (Optional)', email: 'University Email',
      externalPassword: 'Assigned Password for External Instructor', rolesLabel: 'Assigned Roles', saveUser: 'Save Account',
      showPasswords: 'Show Decrypted Passwords', disableUser: 'Disable Account', restoreUser: 'Restore Account',
      allowAccess: 'Override University Work Status', removeAccessOverride: 'Remove Work Override', changePassword: 'Set Password',
      saveRoles: 'Save Roles', userPermissions: 'Individual User Permission Overrides',
      userPermissionsHelp: 'Set role-default, allow, or deny for each permission with effective result auditing.',
      sharedPassword: 'Shared Initial Password', sharedHelp: 'Accepted exclusively by employee accounts that have not completed first login.',
      showShared: 'Show Current Password', newShared: 'New Shared Password', saveShared: 'Save Shared Password',
      rolePermissionsTitle: 'Central Role Permissions', rolePermissionsHelp: 'Default changes apply to all role members unless individually overridden.',
      inherited: 'Use Role Default', allow: 'Allow', deny: 'Deny', effective: 'Effective Result', yes: 'Allowed', no: 'Denied',
      initialPassword: 'Initial Password', currentPassword: 'Current Password', confirmDisable: 'Are you sure you want to disable this account?',
      userCreated: 'Account created successfully. Login ID:', invalidUser: 'Check account details and assigned roles.', noUsers: 'No matching users found.',
      universityTab: 'University Data', universityTitle: 'Central University Directory Sync',
      universityHelp: 'Import and sync faculties, departments, and employees into platform database while preserving local passwords and roles.',
      refreshUniversity: 'Start University Sync', syncRuns: 'Sync History',
      missingEmployees: 'Employees Missing from Last Successful Response (For Review)', syncComplete: 'Sync completed successfully',
      syncIncomplete: 'Sync incomplete', added: 'Added', updated: 'Updated', missing: 'Missing',
      needsTab: 'Training Needs', cancellationsTab: 'Course Cancellations', needsTitle: 'Training Needs',
      needsHelp: 'The Dean submits training needs with official letters directly to Quality.', needTitleAr: 'Arabic Need Title',
      needTitleEn: 'English Title (Optional)', needDescriptionAr: 'Arabic Need Description & Justifications',
      needDescriptionEn: 'English Description (Optional)', officialLetter: 'Accredited Official Letter (PDF or DOCX)',
      submitNeed: 'Submit Need to Quality', cancellationsTitle: 'Approved Course Cancellation Requests',
      cancellationsHelp: 'Approved courses can only be cancelled through signed official letters and Quality approval. Certificates will reflect as revoked.',
      returnRevision: 'Return for Revision', reject: 'Reject', resubmit: 'Resubmit',
      requestCancellation: 'Request Course Cancellation', cancellationReason: 'Cancellation Reason in Arabic',
      reviewNote: 'Quality Note or Decision Justification', officialDocument: 'Official Document',
      revisionRequested: 'Returned for Revision', rejected: 'Rejected', needsEmpty: 'No requests recorded yet.',
      evaluationTab: 'Unified Evaluation Form', evaluationAdminHelp: 'Single shared template. Publishing creates a new version applying to open courses.',
      addQuestion: 'Add Question', addOption: 'Add Option', publishEvaluation: 'Publish New Evaluation Version',
      questionAr: 'Arabic Question', questionEn: 'English Question (Optional)', optionAr: 'Arabic Option',
      optionEn: 'English Option (Optional)', evaluateCourse: 'Evaluate Course', submitEvaluation: 'Submit Official Evaluation',
      evaluationSubmitted: 'Course evaluation survey submitted successfully.', impactTitle: 'Training Impact Plan',
      impactFile: 'Impact Assessment File (PDF or DOCX)', uploadImpact: 'Upload Impact File', impactPending: 'Awaiting Instructor Review',
      impactReturned: 'Returned for Revision by Instructor', impactApproved: 'Officially Approved by Instructor', approveImpact: 'Approve Impact Plan',
      returnImpact: 'Return for Revision', evaluationStats: 'Evaluation Survey Statistics', extendDeadline: 'Extend Evaluation Deadline',
      examRequirement: 'External Exam Requirement', examRequired: 'Exam Required', examNotRequired: 'No Exam Required', changeExamRequirement: 'Modify Exam Requirement',
      certificateGate: 'Accredited Digital Certificate Eligibility Criteria', eligible: 'Congratulations! You have fulfilled all certificate criteria.', notEligible: 'You have not yet fulfilled all criteria for this certificate.',
      gateCourseNotEnded: 'Course has not officially ended', gateNoLectures: 'No sessions recorded yet', gateAttendance: 'Attendance below 75%',
      gateExam: 'Passing exam grade not recorded', gateEvaluation: 'Evaluation survey incomplete', gateImpact: 'Training impact plan not approved',
      verifyCertificate: 'Digital Certificate Verification Portal', verifyHelp: 'Enter the reference number printed on the certificate to verify its authenticity and accreditation by Zarqa University.', certificateReference: 'Certificate Reference Number', verify: 'Verify Now',
      certificateValid: 'Official Accredited & Valid Digital Certificate', certificateRevoked: 'Certificate officially revoked by Zarqa University', certificateNotFound: 'No certificate found with this reference number',
      recipient: 'Certificate Holder', courseName: 'Training Program', instructor: 'Instructor / Trainer', issuedAt: 'Date of Issue & Accreditation',
      reviewProposal: 'Quality Decision on Proposal', resubmitProposal: 'Edit & Resubmit Proposal',
      proposalReviewNote: 'Quality Reason & Feedback in Arabic', proposalUpdated: 'Proposal resubmitted successfully.',
      adminCoursesTab: 'Course Management & Soft Deletion', adminCoursesHelp: 'Administrative soft deletion with certificate visibility controls and restoration capability.',
      disableCourse: 'Disable Course', restoreCourse: 'Restore Course', hideCertificates: 'Hide certificates from verification portal',
      keepCertificates: 'Keep certificates visible', courseDeleteWarning: 'Confirm disabling this course? It will disappear from active course lists.',
      courseActive: 'Active', courseDisabled: 'Soft Disabled', certificates: 'Issued Certificates',
      downloadCertificate: 'Download Accredited PDF Certificate', directVerify: 'Direct Verification'
    }
  };

  const t = key => labels[language][key] || key;

  function api(path, method, data, timeoutOverride) {
    return $.ajax({
      url: apiBase + path,
      method: method,
      cache: false,
      contentType: 'application/json; charset=utf-8',
      data: data ? JSON.stringify(data) : undefined,
      xhrFields: { withCredentials: true },
      timeout: timeoutOverride || (path.includes('university-sync') ? 300000 : 20000)
    });
  }

  function multipart(path, method, formData) {
    return $.ajax({
      url: apiBase + path,
      method: method,
      data: formData,
      processData: false,
      contentType: false,
      xhrFields: { withCredentials: true },
      timeout: 60000
    });
  }

  function notice(selector, value, success) {
    $(selector).removeClass('d-none alert-danger alert-success')
      .addClass(success ? 'alert-success' : 'alert-danger').text(value);
  }

  function show(view) {
    $('#login-view, #forgot-view, #change-view, #verification-view, #dashboard-view').addClass('d-none');
    $('#' + view + '-view').removeClass('d-none');
    const dashboard = view === 'dashboard';
    $('.page-shell').toggleClass('dashboard-mode', dashboard);
    $('.intro, .auth-card').toggleClass('d-none', dashboard);
    $('#status-message').addClass('d-none').text('');
  }

  function role(key) { return (currentAccount?.roles || []).includes(key); }
  function can(key) { return (currentAccount?.permissions || []).includes(key); }
  function name(ar, en) { return language === 'en' && en ? en : ar; }

  function getPrimaryRole(roles) {
    if (!roles || !roles.length) return 'MEMBER';
    const hierarchy = ['ADMIN', 'SYSTEM_ADMIN', 'QA', 'QUALITY_DIRECTOR', 'DEAN', 'QUALITY_STAFF', 'INST', 'INSTRUCTOR', 'EXTERNAL_INSTRUCTOR', 'MEMBER'];
    for (const r of hierarchy) {
      if (roles.includes(r)) return r;
    }
    return roles[0];
  }

  function getRoleLabel(roleKey) {
    return t('role_' + roleKey) || roleKey;
  }

  function getInitialContext(account) {
    const roles = account.roles || [];
    if (roles.includes('ADMIN') || roles.includes('SYSTEM_ADMIN') || can('admin.users.manage')) {
      return 'admin';
    }
    if (roles.includes('QA') || roles.includes('QUALITY_DIRECTOR') || roles.includes('QUALITY_STAFF')) {
      return 'quality';
    }
    if (roles.includes('DEAN') || can('courses.create')) {
      return 'quality';
    }
    if (roles.includes('INST') || roles.includes('INSTRUCTOR')) {
      return 'instructor';
    }
    return 'trainee';
  }

  function stateText(status) {
    return t(({ 1: 'submitted', 2: 'revisionRequested', 3: 'rejected', 4: 'approved', 5: 'inProgress', 6: 'ended', 7: 'cancelled' })[status] || 'submitted');
  }

  function parseDateTime(value) {
    if (!value || typeof value !== 'string') return null;
    let s = value.trim();
    if (!s) return null;

    // Normalize slashes, dots, and multiple spaces
    s = s.replace(/[\/\.]/g, '-').replace(/\s+/g, ' ');

    // Match 1: Time first (e.g. "15:00 20-11-2026" or "15:00 2026-11-20")
    const timeFirstMatch = s.match(/^(\d{1,2}):(\d{2})\s+(\d{1,4})-(\d{1,2})-(\d{1,4})$/);
    if (timeFirstMatch) {
      const [, hh, mm, p1, p2, p3] = timeFirstMatch;
      let y, m, d;
      if (p1.length === 4) { y = p1; m = p2; d = p3; }
      else { d = p1; m = p2; y = p3; }
      const pad = n => String(n).padStart(2, '0');
      const iso = `${y}-${pad(m)}-${pad(d)}T${pad(hh)}:${pad(mm)}:00`;
      const date = new Date(iso);
      return Number.isNaN(date.getTime()) ? null : date.toISOString();
    }

    // Match 2: Date first (e.g. "2026-11-20 15:00" or "20-11-2026 15:00" or "2026-11-20T15:00")
    const dateFirstMatch = s.match(/^(\d{1,4})-(\d{1,2})-(\d{1,4})[T\s]+(\d{1,2}):(\d{2})(?::\d{2})?$/);
    if (dateFirstMatch) {
      const [, p1, p2, p3, hh, mm] = dateFirstMatch;
      let y, m, d;
      if (p1.length === 4) { y = p1; m = p2; d = p3; }
      else { d = p1; m = p2; y = p3; }
      const pad = n => String(n).padStart(2, '0');
      const iso = `${y}-${pad(m)}-${pad(d)}T${pad(hh)}:${pad(mm)}:00`;
      const date = new Date(iso);
      return Number.isNaN(date.getTime()) ? null : date.toISOString();
    }

    // Match 3: Date only (e.g. "2026-11-20" or "20-11-2026")
    const dateOnlyMatch = s.match(/^(\d{1,4})-(\d{1,2})-(\d{1,4})$/);
    if (dateOnlyMatch) {
      const [, p1, p2, p3] = dateOnlyMatch;
      let y, m, d;
      if (p1.length === 4) { y = p1; m = p2; d = p3; }
      else { d = p1; m = p2; y = p3; }
      const pad = n => String(n).padStart(2, '0');
      const iso = `${y}-${pad(m)}-${pad(d)}T09:00:00`;
      const date = new Date(iso);
      return Number.isNaN(date.getTime()) ? null : date.toISOString();
    }

    const fallback = new Date(s);
    return Number.isNaN(fallback.getTime()) ? null : fallback.toISOString();
  }

  function localDateTime(value) {
    if (!value) return '';
    const date = new Date(value), pad = n => String(n).padStart(2, '0');
    return date.getFullYear() + '-' + pad(date.getMonth() + 1) + '-' + pad(date.getDate()) + ' ' +
      pad(date.getHours()) + ':' + pad(date.getMinutes());
  }

  function translate() {
    document.documentElement.lang = language;
    document.documentElement.dir = language === 'ar' ? 'rtl' : 'ltr';
    $('#bootstrap-style').attr('href', language === 'ar' ? '/vendor/bootstrap.rtl.min.css' : '/vendor/bootstrap.min.css');
    $('[data-i18n]').each(function () { $(this).text(t($(this).data('i18n'))); });
    $('[data-i18n-placeholder]').each(function () { $(this).attr('placeholder', t($(this).data('i18n-placeholder'))); });
    $('#language-toggle').text(language === 'ar' ? 'English' : 'العربية');
    if (signedIn) {
      renderCourseList();
      renderCourseSelects();
      if (currentAccount) {
        $('#header-user-name').text(name(currentAccount.nameAr, currentAccount.nameEn));
        $('#header-user-badge').text(getRoleLabel(getPrimaryRole(currentAccount.roles)));
      }
    }
  }

  // ================= Auth & Context Navigation =================
  function showAccount(account) {
    currentAccount = account;
    signedIn = true;
    if (account.preferredLanguage === 'ar' || account.preferredLanguage === 'en') {
      language = account.preferredLanguage;
    }
    translate();

    if (account.mustChangePassword) {
      show('change');
      return;
    }

    // User header widget
    $('#user-header-widget').removeClass('d-none');
    $('#header-user-name').text(name(account.nameAr, account.nameEn));
    const primaryRole = getPrimaryRole(account.roles);
    $('#header-user-badge').text(getRoleLabel(primaryRole));

    // Context Navigation Buttons Visibility
    const isQuality = role('QUALITY_DIRECTOR') || role('QUALITY_STAFF') || role('QA') || role('DEAN') || can('courses.approve') || can('training_needs.view');
    const isAdmin = role('ADMIN') || role('SYSTEM_ADMIN') || can('admin.users.manage');
    const isAnalytics = isAdmin || isQuality;
    const isVault = isAdmin || isQuality;

    $('#nav-quality-btn').toggleClass('d-none', !isQuality);
    $('#nav-analytics-btn').toggleClass('d-none', !isAnalytics);
    $('#nav-vault-btn').toggleClass('d-none', !isVault);
    $('#nav-admin-btn').toggleClass('d-none', !isAdmin);
    $('#role-context-nav').removeClass('d-none');

    const canHistorical = role('ADMIN') || role('QA') || role('QUALITY_DIRECTOR') || can('courses.approve') || can('admin.courses.manage');
    $('[data-quality-tab="historical"]').toggleClass('d-none', !canHistorical);

    // Dynamic Role Routing: Direct user to their primary active workspace upon login
    const initialContext = getInitialContext(account);
    switchContext(initialContext);
    show('dashboard');

    // Preload courses & metadata
    loadCourses();
  }

  function switchContext(context) {
    $('.context-btn').removeClass('active');
    $('.context-btn[data-context="' + context + '"]').addClass('active');
    $('.workspace-tab-content').addClass('d-none');
    $('#course-details-panel').addClass('d-none');
    currentCourse = null;
    $('#' + context + '-workspace').removeClass('d-none');

    if (context === 'trainee') {
      loadCourses();
      loadMyCertificates();
      loadMyProfileSummary();
    } else if (context === 'instructor') {
      loadInstructorCourses();
    } else if (context === 'quality') {
      const activeTab = $('[data-quality-tab].active').data('quality-tab') || 'proposals';
      $('#quality-workspace .workspace-panel').addClass('d-none');
      $('#proposals-panel, #new-proposal-panel, #needs-panel, #cancellations-panel, #historical-panel').addClass('d-none');
      $('#' + activeTab + '-panel').removeClass('d-none');
      loadQualityProposals();
      loadQualityRequests();
    } else if (context === 'analytics') {
      loadAnalyticsDashboard();
    } else if (context === 'vault') {
      loadVaultDocuments();
    } else if (context === 'admin') {
      const activeAdminTab = $('[data-admin-tab].active').data('admin-tab') || 'hints';
      $('#admin-workspace .workspace-panel').addClass('d-none');
      $('#' + activeAdminTab + '-panel').removeClass('d-none');
      loadAdmin();
    }
  }

  $(document).on('click', '.context-btn', function () {
    switchContext($(this).data('context'));
  });

  function loadAccount() {
    api('/api/auth/me', 'GET')
      .done(showAccount)
      .fail(() => {
        signedIn = false;
        $('#user-header-widget, #role-context-nav').addClass('d-none');
        show('login');
      });
  }

  // ================= Trainee Workspace Logic =================
  function loadMyProfileSummary() {
    api('/api/users/me/profile', 'GET').done(profile => {
      $('#kpi-completed-count').text(profile.completedCoursesCount || 0);
      $('#kpi-hours-count').text(profile.totalEarnedHours || 0);
      $('#kpi-certs-count').text(profile.verifiedCertificatesCount || 0);
    });
  }

  function loadCourses() {
    api('/api/courses', 'GET').done(rows => {
      courses = rows;
      $('#kpi-enrolled-count').text(courses.filter(c => c.isEnrolled).length);
      renderCourseList();
      populateAnalyticsCourseSelect();
    });
    api('/api/courses/tracks', 'GET').done(rows => {
      tracks = rows;
      renderCourseSelects();
    });
    api('/api/courses/faculties', 'GET').done(rows => {
      faculties = rows;
      renderCourseSelects();
      renderAdminFacultySelect();
      renderNeedFacultySelect();
    });
  }

  function renderCourseSelects() {
    const track = $('#course-track, #hist-track');
    track.empty();
    tracks.forEach(x => $('<option>').val(x.id).text(name(x.nameAr, x.nameEn)).appendTo(track));

    const faculty = $('#course-faculty, #hist-faculty');
    faculty.empty();
    $('<option>').val('').text(language === 'ar' ? 'دورة عامة لجميع الكليات' : 'General course for all faculties').appendTo(faculty);
    faculties.forEach(x => $('<option>').val(x.id).text(name(x.nameAr, x.nameEn)).appendTo(faculty));

    if (role('DEAN') && !role('QUALITY_DIRECTOR') && !role('ADMIN')) {
      faculty.val(currentAccount?.facultyId || '').prop('disabled', true);
      $('#course-quality-operated').prop('disabled', true);
      $('#course-deadline').prop('disabled', true);
    }
  }

  function renderCourseList() {
    const filter = $('#course-filter-select').val();
    const list = $('#course-list').empty();
    let displayList = courses;

    if (filter === 'enrolled') {
      displayList = courses.filter(c => c.isEnrolled);
    } else if (filter === 'available') {
      displayList = courses.filter(c => c.canSelfEnroll && !c.isEnrolled);
    }

    if (!displayList.length) {
      $('<p class="muted mb-0">').text(t('noCourses')).appendTo(list);
      return;
    }

    displayList.forEach(course => {
      const card = $('<article class="course-card">').appendTo(list);
      const head = $('<div class="course-card-head">').appendTo(card);
      $('<h4>').text(name(course.titleAr, course.titleEn)).appendTo(head);
      $('<span class="badge text-bg-light border">').text(stateText(course.status)).appendTo(head);
      $('<p class="muted mb-2">').text(course.actualInstructorNameAr).appendTo(card);
      const actions = $('<div class="d-flex gap-2 flex-wrap">').appendTo(card);
      $('<button type="button" class="btn btn-sm btn-outline-primary course-view">').data('id', course.id).text(t('view')).appendTo(actions);
      if (course.canSelfEnroll && can('courses.self_enroll') && !course.isEnrolled) {
        $('<button type="button" class="btn btn-sm btn-primary course-enroll">').data('id', course.id).text(t('enroll')).appendTo(actions);
      }
      if (course.isEnrolled) {
        $('<span class="badge text-bg-success align-self-center">').text(t('registered')).appendTo(actions);
      }
    });
  }

  $('#course-filter-select').on('change', renderCourseList);

  function loadMyCertificates() {
    const container = $('#my-certificates-list').empty();
    api('/api/certificates/my', 'GET').done(certs => {
      if (!certs.length) {
        $('<p class="muted">').text(t('noRows')).appendTo(container);
        return;
      }
      certs.forEach(cert => {
        const card = $('<div class="certificate-card">').appendTo(container);
        const header = $('<div class="certificate-header">').appendTo(card);
        $('<div class="cert-title">').text(name(cert.courseTitleAr, cert.courseTitleEn)).appendTo(header);
        $('<div class="cert-ref">').text('REF: ' + cert.certificateReference).appendTo(header);

        const body = $('<div class="certificate-body">').appendTo(card);
        $('<div class="certificate-meta-row">')
          .append($('<span class="certificate-meta-label">').text(t('instructor') + ':'))
          .append($('<span class="certificate-meta-value">').text(cert.instructorNameAr)).appendTo(body);
        $('<div class="certificate-meta-row">')
          .append($('<span class="certificate-meta-label">').text(t('issuedAt') + ':'))
          .append($('<span class="certificate-meta-value">').text(new Date(cert.issuedAtUtc).toLocaleDateString(language === 'ar' ? 'ar-JO' : 'en-GB'))).appendTo(body);

        const footer = $('<div class="certificate-footer">').appendTo(card);
        const badge = $('<span class="' + (cert.status === 0 ? 'cert-badge-valid' : 'cert-badge-revoked') + '">')
          .text(cert.status === 0 ? t('certificateValid') : t('certificateRevoked')).appendTo(footer);

        const btnGroup = $('<div class="d-flex gap-2">').appendTo(footer);
        $('<a class="btn btn-sm btn-gold" target="_blank">')
          .attr('href', apiBase + '/api/certificates/' + cert.id + '/download')
          .text(t('downloadCertificate')).appendTo(btnGroup);
        $('<button type="button" class="btn btn-sm btn-outline-primary direct-verify-btn">')
          .data('ref', cert.certificateReference)
          .text(t('directVerify')).appendTo(btnGroup);
      });
    }).fail(() => $('<p class="muted">').text(t('noRows')).appendTo(container));
  }

  $(document).on('click', '.direct-verify-btn', function () {
    const ref = $(this).data('ref');
    show('verification');
    $('#verification-reference').val(ref);
    verifyCertificate(ref);
  });

  // ================= Instructor Workspace Logic =================
  function loadInstructorCourses() {
    const list = $('#instructor-courses-list').empty();
    const alertBox = $('#instructor-empty-alert');
    const myInstructorCourses = courses.filter(c =>
      c.instructorUserId === currentAccount?.id || (c.qualityOperated && (role('QUALITY_DIRECTOR') || role('QUALITY_STAFF') || role('ADMIN')))
    );

    if (!myInstructorCourses.length) {
      alertBox.removeClass('d-none');
      return;
    }
    alertBox.addClass('d-none');

    myInstructorCourses.forEach(course => {
      const card = $('<article class="course-card">').appendTo(list);
      const head = $('<div class="course-card-head">').appendTo(card);
      $('<h4>').text(name(course.titleAr, course.titleEn)).appendTo(head);
      $('<span class="badge text-bg-primary">').text(stateText(course.status)).appendTo(head);
      $('<p class="muted mb-2">').text((course.plannedHours || 0) + ' ' + (language === 'ar' ? 'ساعة معتمدة' : 'hours')).appendTo(card);
      $('<button type="button" class="btn btn-sm btn-primary course-view">')
        .data('id', course.id).text(language === 'ar' ? 'إدارة الجلسات والحضور والامتحان' : 'Manage Sessions & Grading').appendTo(card);
    });
  }

  // ================= Course Details Panel =================
  function showCourseDetails(id) {
    api('/api/courses/' + id, 'GET').done(detail => {
      currentCourse = detail;
      $('#course-details-panel').removeClass('d-none');
      renderCourseDetails();
      $('#course-details-panel')[0].scrollIntoView({ behavior: 'smooth', block: 'start' });
    }).fail(xhr => notice('#course-message', xhr.responseJSON?.message || t('failed')));
  }

  function canOperate(course) {
    return role('ADMIN') || course.instructorUserId === currentAccount?.id ||
      ((role('QUALITY_DIRECTOR') || role('QUALITY_STAFF')) && course.qualityOperated);
  }

  function renderCourseDetails() {
    const detail = currentCourse;
    if (!detail) return;
    const course = detail.course, box = $('#course-detail-content').empty();

    $('#detail-title').text(name(course.titleAr, course.titleEn));
    $('#detail-meta').text(stateText(course.status) + ' · ' + course.actualInstructorNameAr + ' · ' +
      (detail.plannedHours || 0) + ' ' + (language === 'ar' ? 'ساعة معتمدة' : 'hours'));

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

    if (course.status === 2 && course.createdByUserId === currentAccount?.id && can('courses.create')) {
      $('<button class="btn btn-outline-primary mb-4 edit-proposal" type="button">').text(t('resubmitProposal')).appendTo(box);
    }

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
      const col = $('<div class="col-md-5">').appendTo(form);
      $('<label class="form-label">').text(t('lectureTitle')).appendTo(col);
      $('<input class="form-control" name="title" required maxlength="300">').appendTo(col);
      const dateCol = $('<div class="col-md-4">').appendTo(form);
      $('<label class="form-label">').text(t('lectureDate')).appendTo(dateCol);
      $('<input class="form-control" name="date" placeholder="2026-11-20 09:00">').appendTo(dateCol);
      $('<div class="col-md-3">').append($('<button class="btn btn-primary w-100" type="submit">').text(t('addLecture'))).appendTo(form);
      if (can('courses.end')) {
        $('<button type="button" class="btn btn-outline-danger mb-4 end-course">').text(t('endCourse')).appendTo(box);
      }
    }

    $('<h4 class="section-title">').text(t('lectures')).appendTo(box);
    if (!detail.lectures.length) $('<p class="muted">').text(t('noLectures')).appendTo(box);
    detail.lectures.forEach(lecture => {
      const card = $('<div class="lecture-card">').appendTo(box);
      const head = $('<div class="d-flex justify-content-between align-items-center gap-2 flex-wrap">').appendTo(card);
      $('<h5>').text(lecture.sequenceNumber + '. ' + name(lecture.titleAr, lecture.titleEn)).appendTo(head);
      if (lecture.startsAtUtc) {
        $('<small class="muted">').text(new Date(lecture.startsAtUtc).toLocaleString(language === 'ar' ? 'ar-JO' : 'en-GB')).appendTo(head);
      }
      if (operator && can('courses.lectures.manage') && (course.status === 4 || course.status === 5)) {
        $('<button type="button" class="btn btn-sm btn-outline-primary activate-code">').data('id', lecture.id).text(t('activateCode')).appendTo(card);
      }
      if (course.isEnrolled && can('courses.attendance.enter_code') && (course.status === 4 || course.status === 5)) {
        const form = $('<form class="code-form d-flex gap-2 mt-3">').data('id', lecture.id).appendTo(card);
        $('<input class="form-control code-input" inputmode="numeric" maxlength="4" pattern="[0-9]{4}">').attr('placeholder', t('attendanceCode')).appendTo(form);
        $('<button type="submit" class="btn btn-primary">').text(t('submitCode')).appendTo(form);
      }
      if (course.isEnrolled && lecture.isPresent !== null) {
        $('<span class="badge text-bg-success ms-2">').text(lecture.isPresent ? t('attendance') + ' ✓' : '—').appendTo(card);
      }
      if (operator && can('courses.attendance.correct') && detail.participants.length) {
        const table = $('<div class="table-responsive mt-3"><table class="table table-sm"><tbody></tbody></table></div>').appendTo(card);
        const body = table.find('tbody');
        detail.participants.forEach(person => {
          const row = $('<tr>').appendTo(body);
          $('<td>').text(name(person.nameAr, person.nameEn)).appendTo(row);
          const cell = $('<td>').appendTo(row);
          const present = detail.attendance.some(x => x.lectureId === lecture.id && x.enrollmentId === person.enrollmentId && x.isPresent);
          $('<input type="checkbox" class="form-check-input manual-attendance">').prop('checked', present)
            .data('lecture', lecture.id).data('enrollment', person.enrollmentId).appendTo(cell);
        });
      }
    });

    if (course.hasExam) {
      $('<h4 class="section-title">').text(t('exam')).appendTo(box);
      if (detail.examNameAr) {
        $('<p>').text(name(detail.examNameAr, detail.examNameEn)).appendTo(box);
      } else if (operator && can('courses.exam.record')) {
        const form = $('<form id="exam-form" class="d-flex gap-2 mb-3">').appendTo(box);
        $('<input name="name" class="form-control" required>').attr('placeholder', t('examName')).appendTo(form);
        $('<button type="submit" class="btn btn-primary">').text(t('createExam')).appendTo(form);
      }
      if (course.isEnrolled) {
        $('<p>').text(t('exam') + ': ' + t(({ 1: 'passed', 2: 'failed' })[detail.myExamOutcome] || 'notRecorded')).appendTo(box);
      }
      if (operator && can('courses.exam.record') && detail.examNameAr) {
        detail.participants.forEach(person => {
          const line = $('<div class="d-flex align-items-center gap-3 mb-2">').appendTo(box);
          $('<span class="person-name">').text(name(person.nameAr, person.nameEn)).appendTo(line);
          const select = $('<select class="form-select exam-outcome">').data('enrollment', person.enrollmentId).appendTo(line);
          [[0, 'notRecorded'], [1, 'passed'], [2, 'failed']].forEach(([value, label]) => $('<option>').val(value).text(t(label)).appendTo(select));
          select.val(person.examOutcome);
        });
      }
    }

    if (can('courses.participants.select')) {
      $('<h4 class="section-title">').text(t('participants')).appendTo(box);
      const form = $('<div class="d-flex gap-2 mb-3">').appendTo(box);
      $('<input id="participant-search" class="form-control">').attr('placeholder', t('searchParticipant')).appendTo(form);
      $('<select id="participant-results" class="form-select"><option value=""></option></select>').appendTo(form);
      $('<button id="add-participant" type="button" class="btn btn-primary">').text(t('addParticipant')).appendTo(form);
    }

    if (detail.participants.length && (role('QUALITY_DIRECTOR') || role('ADMIN') || operator)) {
      const table = $('<div class="table-responsive"><table class="table"><thead><tr><th></th><th></th><th></th></tr></thead><tbody></tbody></table></div>').appendTo(box);
      table.find('th').eq(0).text(t('participants'));
      table.find('th').eq(1).text(t('attendance'));
      table.find('th').eq(2).text(t('exam'));
      detail.participants.forEach(person => {
        const row = $('<tr>').appendTo(table.find('tbody'));
        const nameCell = $('<td>').appendTo(row);
        $('<a href="#" class="open-profile-link text-decoration-none">').data('userid', person.userId).text(name(person.nameAr, person.nameEn)).appendTo(nameCell);
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

    if (can('evaluation.statistics.view') && (role('ADMIN') || role('QUALITY_DIRECTOR') || operator)) {
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

  // ================= Quality & Dean Workspace Logic =================
  $('[data-quality-tab]').on('click', function () {
    const tab = $(this).data('quality-tab');
    $('[data-quality-tab]').removeClass('active');
    $(this).addClass('active');
    $('#quality-workspace .workspace-panel').addClass('d-none');
    $('#proposals-panel, #new-proposal-panel, #needs-panel, #cancellations-panel, #historical-panel').addClass('d-none');
    $('#' + tab + '-panel').removeClass('d-none');
    $('#quality-message').addClass('d-none');
  });

  function loadQualityProposals() {
    const list = $('#quality-proposals-list').empty();
    const proposals = courses.filter(c => c.status === 1 || c.status === 2 || c.status === 3);
    if (!proposals.length) {
      $('<p class="muted">').text(t('noRows')).appendTo(list);
      return;
    }
    proposals.forEach(course => {
      const card = $('<article class="course-card">').appendTo(list);
      const head = $('<div class="course-card-head">').appendTo(card);
      $('<h4>').text(name(course.titleAr, course.titleEn)).appendTo(head);
      $('<span class="badge text-bg-warning">').text(stateText(course.status)).appendTo(head);
      $('<p class="muted mb-2">').text(course.actualInstructorNameAr).appendTo(card);
      $('<button type="button" class="btn btn-sm btn-outline-primary course-view">')
        .data('id', course.id).text(t('view')).appendTo(card);
    });
  }

  function loadQualityRequests() {
    api('/api/training-needs', 'GET').done(rows => {
      qualityNeeds = rows;
      renderQualityNeeds();
    });
    api('/api/course-cancellations', 'GET').done(rows => {
      cancellationRequests = rows;
      renderCancellations();
    });
  }

  function renderQualityNeeds() {
    const container = $('#needs-list').empty();
    if (!qualityNeeds.length) {
      $('<p class="muted">').text(t('needsEmpty')).appendTo(container);
      return;
    }
    qualityNeeds.forEach(need => {
      const card = $('<div class="request-card">').appendTo(container);
      $('<h4>').text(name(need.titleAr, need.titleEn)).appendTo(card);
      $('<p>').text(name(need.descriptionAr, need.descriptionEn)).appendTo(card);
      $('<a class="btn btn-sm btn-outline-primary me-2 mb-3">')
        .attr('href', apiBase + '/api/training-needs/' + need.id + '/file').text(t('officialLetter')).appendTo(card);
      if (can('training_needs.review') && need.status === 1) {
        const reviewBox = $('<div class="d-flex gap-2 align-items-center mt-2 flex-wrap">').appendTo(card);
        $('<textarea class="form-control need-review-note" rows="1" style="max-width: 20rem">').attr('placeholder', t('reviewNote')).appendTo(reviewBox);
        $('<button class="btn btn-sm btn-outline-success review-need">').data('id', need.id).data('status', 2).text(t('approved')).appendTo(reviewBox);
        $('<button class="btn btn-sm btn-outline-warning review-need">').data('id', need.id).data('status', 3).text(t('returnRevision')).appendTo(reviewBox);
        $('<button class="btn btn-sm btn-outline-danger review-need">').data('id', need.id).data('status', 4).text(t('reject')).appendTo(reviewBox);
      }
    });
  }

  function renderCancellations() {
    const container = $('#cancellations-list').empty();
    if (!cancellationRequests.length) {
      $('<p class="muted">').text(t('needsEmpty')).appendTo(container);
      return;
    }
    cancellationRequests.forEach(req => {
      const card = $('<div class="request-card">').appendTo(container);
      $('<h4>').text(req.courseTitleAr).appendTo(card);
      $('<p>').text(req.reasonAr).appendTo(card);
      $('<a class="btn btn-sm btn-outline-primary me-2 mb-3">')
        .attr('href', apiBase + '/api/course-cancellations/' + req.id + '/file').text(t('officialDocument')).appendTo(card);
      if (can('course_cancellations.review') && req.status === 1) {
        const reviewBox = $('<div class="d-flex gap-2 align-items-center mt-2 flex-wrap">').appendTo(card);
        $('<textarea class="form-control cancellation-review-note" rows="1" style="max-width: 20rem">').attr('placeholder', t('reviewNote')).appendTo(reviewBox);
        $('<button class="btn btn-sm btn-outline-success review-cancellation">').data('id', req.id).data('status', 2).text(t('approved')).appendTo(reviewBox);
        $('<button class="btn btn-sm btn-outline-danger review-cancellation">').data('id', req.id).data('status', 3).text(t('reject')).appendTo(reviewBox);
      }
    });
  }

  // Historical Course Backfill Dynamic Participants
  $('#hist-user-search').on('input', function () {
    searchDirectory($(this).val(), $('#hist-faculty').val(), $('#hist-user-results'));
  });

  $('#hist-add-participant-btn').on('click', function () {
    const select = $('#hist-user-results');
    const option = select.find(':selected');
    const user = option.data('user');
    if (!user) return;
    if (historicalParticipants.some(x => x.id === user.id)) return;

    historicalParticipants.push(user);
    renderHistoricalParticipants();
  });

  function renderHistoricalParticipants() {
    const tbody = $('#hist-participants-table').empty();
    historicalParticipants.forEach((user, idx) => {
      const row = $('<tr>').data('user-id', user.id).appendTo(tbody);
      $('<td>').text(name(user.nameAr, user.nameEn) + ' (' + user.loginName + ')').appendTo(row);
      $('<td>').append($('<input type="number" min="0" value="4" class="form-control form-control-sm hist-attended-input" style="width: 80px">')).appendTo(row);
      const examCell = $('<td>').appendTo(row);
      $('<select class="form-select form-select-sm hist-exam-select">')
        .append($('<option value="0">').text('بلا امتحان'))
        .append($('<option value="1" selected>').text('ناجح'))
        .append($('<option value="2">').text('راسب')).appendTo(examCell);
      $('<td>').append($('<input type="checkbox" class="form-check-input hist-eval-check" checked>')).appendTo(row);
      $('<td>').append($('<input type="checkbox" class="form-check-input hist-impact-check" checked>')).appendTo(row);
      $('<td>').append($('<button type="button" class="btn btn-sm btn-outline-danger hist-remove-participant">').data('idx', idx).text(t('remove'))).appendTo(row);
    });
  }

  $('#hist-participants-table').on('click', '.hist-remove-participant', function () {
    const idx = $(this).data('idx');
    historicalParticipants.splice(idx, 1);
    renderHistoricalParticipants();
  });

  $('#historical-form').on('submit', function (e) {
    e.preventDefault();
    const lectures = $('#hist-lectures').val().split('\n').map(s => s.trim()).filter(Boolean);
    if (!lectures.length) {
      notice('#quality-message', 'أدخل جلسات الدورة (جلسة لكل سطر)');
      return;
    }

    const participants = [];
    $('#hist-participants-table tr').each(function () {
      const row = $(this);
      participants.push({
        userId: row.data('user-id'),
        attendedLectureCount: Number(row.find('.hist-attended-input').val()) || 0,
        examOutcome: Number(row.find('.hist-exam-select').val()) || 0,
        evaluationCompleted: row.find('.hist-eval-check').is(':checked'),
        impactApproved: row.find('.hist-impact-check').is(':checked')
      });
    });

    const payload = {
      titleAr: $('#hist-title-ar').val().trim(),
      titleEn: $('#hist-title-en').val().trim() || null,
      trainingTrackId: $('#hist-track').val(),
      facultyId: $('#hist-faculty').val() || null,
      actualInstructorNameAr: $('#hist-instructor').val().trim(),
      plannedHours: Number($('#hist-hours').val()),
      startDateUtc: parseDateTime($('#hist-start').val()) || new Date().toISOString(),
      endDateUtc: parseDateTime($('#hist-end').val()) || new Date().toISOString(),
      lectureTitles: lectures,
      hasExam: $('#hist-has-exam').is(':checked'),
      participants: participants
    };

    const button = $(this).find('button[type=submit]').prop('disabled', true);
    api('/api/courses/historical', 'POST', payload)
      .done(res => {
        notice('#quality-message', t('historicalSaved') + ' (الشهادات الصادرة: ' + res.certificatesIssued + ')', true);
        $('#historical-form')[0].reset();
        historicalParticipants = [];
        renderHistoricalParticipants();
        loadCourses();
        loadMyCertificates();
      })
      .fail(xhr => notice('#quality-message', xhr.responseJSON?.message || t('failed')))
      .always(() => button.prop('disabled', false));
  });

  // ================= Analytics Workspace Logic =================
  function populateAnalyticsCourseSelect() {
    const select = $('#stat-course-select').empty();
    courses.forEach(c => $('<option>').val(c.id).text(name(c.titleAr, c.titleEn)).appendTo(select));
  }

  function loadAnalyticsDashboard() {
    api('/api/analytics/dashboard', 'GET').done(data => {
      $('#stat-total-courses').text(data.totalCourses || 0);
      $('#stat-active-courses').text(data.activeCourses || 0);
      $('#stat-ended-courses').text(data.endedCourses || 0);
      $('#stat-total-hours').text(data.totalTrainingHours || 0);
      $('#stat-valid-certs').text(data.validCertificates || 0);
      $('#stat-pass-rate').text(data.examPassRatePercentage + '%');

      // Faculty breakdown
      const facBody = $('#stat-faculties-body').empty();
      (data.faculties || []).forEach(f => {
        const row = $('<tr>').appendTo(facBody);
        $('<td>').text(name(f.facultyNameAr, f.facultyNameEn)).appendTo(row);
        $('<td>').text(f.courseCount).appendTo(row);
        $('<td>').text(f.totalParticipants).appendTo(row);
        $('<td>').text(f.validCertificates).appendTo(row);
      });

      // Track breakdown
      const trackBody = $('#stat-tracks-body').empty();
      (data.tracks || []).forEach(tr => {
        const row = $('<tr>').appendTo(trackBody);
        $('<td>').text(name(tr.trackNameAr, tr.trackNameEn)).appendTo(row);
        $('<td>').text(tr.courseCount).appendTo(row);
        $('<td>').text(tr.totalHours).appendTo(row);
      });

      // Cancelled courses table
      const cancelBody = $('#stat-cancellations-body').empty();
      (data.cancelledCourses || []).forEach(c => {
        const row = $('<tr>').appendTo(cancelBody);
        $('<td>').text(name(c.courseTitleAr, c.courseTitleEn)).appendTo(row);
        $('<td>').text(c.facultyNameAr || '—').appendTo(row);
        $('<td>').text(c.reasonAr).appendTo(row);
        $('<td>').text(c.cancelledAtUtc ? new Date(c.cancelledAtUtc).toLocaleDateString() : '—').appendTo(row);
        const docCell = $('<td>').appendTo(row);
        if (c.officialDocumentId) {
          $('<a class="btn btn-sm btn-outline-primary" target="_blank">')
            .attr('href', apiBase + '/api/vault/documents/' + c.officialDocumentId + '/download')
            .text(t('download')).appendTo(docCell);
        } else {
          docCell.text('—');
        }
      });
    });
  }

  $('#load-course-sessions-btn').on('click', function () {
    const courseId = $('#stat-course-select').val();
    if (!courseId) return;
    const container = $('#stat-sessions-container').empty();

    api('/api/analytics/courses/' + courseId + '/sessions', 'GET').done(sessions => {
      if (!sessions.length) {
        $('<p class="muted">').text(t('noLectures')).appendTo(container);
        return;
      }
      const table = $('<table class="table table-bordered table-sm align-middle"><thead><tr><th>#</th><th>عنوان الجلسة</th><th>موعد الانعقاد</th><th>عدد الحضور</th><th>إجمالي المسجلين</th><th>نسبة الحضور</th></tr></thead><tbody></tbody></table>').appendTo(container);
      const tbody = table.find('tbody');
      sessions.forEach(s => {
        const row = $('<tr>').appendTo(tbody);
        $('<td>').text(s.sequenceNumber).appendTo(row);
        $('<td>').text(name(s.titleAr, s.titleEn)).appendTo(row);
        $('<td>').text(s.startsAtUtc ? new Date(s.startsAtUtc).toLocaleString() : '—').appendTo(row);
        $('<td>').text(s.attendedCount).appendTo(row);
        $('<td>').text(s.totalEnrolled).appendTo(row);
        $('<td>').text(s.attendancePercentage + '%').appendTo(row);
      });
    });
  });

  // ================= Central Document Vault Logic =================
  function loadVaultDocuments() {
    const tbody = $('#vault-documents-body').empty();
    api('/api/vault/documents', 'GET').done(docs => {
      if (!docs.length) {
        $('<tr>').append($('<td colspan="5" class="text-center muted">').text(t('noRows'))).appendTo(tbody);
        return;
      }
      docs.forEach(doc => {
        const row = $('<tr>').appendTo(tbody);
        $('<td>').append($('<span class="badge bg-secondary">').text(doc.documentType)).appendTo(row);
        $('<td>').text(doc.originalFileName).appendTo(row);
        $('<td>').text(new Date(doc.createdAtUtc).toLocaleDateString(language === 'ar' ? 'ar-JO' : 'en-GB')).appendTo(row);
        $('<td>').text(doc.uploadedByUserName || 'System').appendTo(row);
        $('<td>').append($('<a class="btn btn-sm btn-outline-primary" target="_blank">')
          .attr('href', apiBase + '/api/vault/documents/' + doc.id + '/download').text(t('download'))).appendTo(row);
      });
    }).fail(() => $('<tr>').append($('<td colspan="5" class="text-center muted">').text(t('noRows'))).appendTo(tbody));
  }

  // ================= Admin Workspace Logic =================
  $('[data-admin-tab]').on('click', function () {
    const tab = $(this).data('admin-tab');
    $('[data-admin-tab]').removeClass('active');
    $(this).addClass('active');
    $('#admin-workspace .workspace-panel').addClass('d-none');
    $('#' + tab + '-panel').removeClass('d-none');
    $('#admin-message').addClass('d-none');

    if (tab === 'audit-logs') loadAuditLogs();
    if (tab === 'admin-courses') loadAdminCourses();
  });

  function loadAdmin() {
    if (can('admin.hints.manage')) {
      api('/api/settings/hints', 'GET').done(rows => { hints = rows; renderHints(); }).fail(() => notice('#admin-message', t('connectionError')));
    }
    if (can('admin.password_policy.manage')) {
      api('/api/settings/password-policy', 'GET').done(p => {
        $('#policy-min').val(p.minimumLength);
        $('#policy-max').val(p.maximumLength);
        $('#policy-spaces').prop('checked', p.allowSpaces);
        $('#policy-mix').prop('checked', p.requireMixedCharacterClasses);
        $('#policy-common').prop('checked', p.blockCommonPasswords);
      });
    }
    if (can('admin.certificate_templates.manage')) {
      api('/api/settings/certificate-templates', 'GET').done(rows => { templates = rows; renderTemplates(); });
    }
    if (can('admin.users.manage')) {
      api('/api/admin/roles', 'GET').done(rows => { adminRoles = rows; renderRoleSelect(); renderRoleChecks('#create-role-list', []); });
      searchAdminUsers();
    }
    if (can('admin.university_sync.run')) loadSyncData();
    if (can('evaluation.manage')) loadEvaluationEditor();
    if (can('admin.courses.manage')) loadAdminCourses();
  }

  function loadAuditLogs() {
    const tbody = $('#audit-logs-body').empty();
    api('/api/vault/audit-logs', 'GET').done(logs => {
      if (!logs.length) {
        $('<tr>').append($('<td colspan="6" class="text-center muted">').text(t('noRows'))).appendTo(tbody);
        return;
      }
      logs.forEach(log => {
        const row = $('<tr>').appendTo(tbody);
        $('<td>').text(new Date(log.timestampUtc).toLocaleString(language === 'ar' ? 'ar-JO' : 'en-GB')).appendTo(row);
        $('<td>').text(log.actorName || log.actorUserId || 'System').appendTo(row);
        $('<td>').append($('<span class="badge-audit-action">').text(log.actionKey)).appendTo(row);
        $('<td>').text(log.entityName || '—').appendTo(row);
        $('<td>').append($('<code>').text(log.ipAddress || '—')).appendTo(row);
        $('<td>').text(log.details || '—').appendTo(row);
      });
    }).fail(() => $('<tr>').append($('<td colspan="6" class="text-center muted">').text(t('noRows'))).appendTo(tbody));
  }

  $('#dispatch-initial-emails-btn').on('click', function () {
    const button = $(this).prop('disabled', true);
    api('/api/admin/users/dispatch-initial-emails', 'POST')
      .done(res => {
        notice('#dispatch-result', t('dispatchSuccess') + ' (' + res.message + ')', true);
      })
      .fail(xhr => notice('#dispatch-result', xhr.responseJSON?.message || t('failed')))
      .always(() => button.prop('disabled', false));
  });

  // Profile Modal logic
  $('#open-my-profile').on('click', function () {
    openUserProfile(currentAccount?.id);
  });

  $(document).on('click', '.open-profile-link', function (e) {
    e.preventDefault();
    openUserProfile($(this).data('userid'));
  });

  function openUserProfile(userId) {
    const modalEl = document.getElementById('profile-modal');
    const modal = new bootstrap.Modal(modalEl);
    modal.show();

    const body = $('#profile-modal-body').html('<div class="text-center py-4"><div class="spinner-border text-primary" role="status"></div></div>');
    const endpoint = userId ? '/api/users/' + userId + '/profile' : '/api/users/me/profile';

    api(endpoint, 'GET').done(p => {
      body.empty();
      const hero = $('<div class="profile-hero">').appendTo(body);
      $('<div class="profile-avatar">').text(p.nameAr ? p.nameAr.charAt(0) : 'U').appendTo(hero);
      const info = $('<div class="profile-info">').appendTo(hero);
      $('<h3>').text(name(p.nameAr, p.nameEn)).appendTo(info);
      $('<p>').text(t('employeeNumber') + ': ' + p.loginName + (p.facultyNameAr ? ' · ' + p.facultyNameAr : '')).appendTo(info);
      const rolesBadge = $('<div>').appendTo(info);
      (p.roles || []).forEach(r => $('<span class="badge bg-gold me-1">').text(getRoleLabel(r)).appendTo(rolesBadge));

      const stats = $('<div class="profile-stats-row">').appendTo(body);
      $('<div class="profile-stat-box">').append($('<span class="num">').text(p.completedCoursesCount)).append($('<span class="lbl">').text(t('kpiCompleted'))).appendTo(stats);
      $('<div class="profile-stat-box">').append($('<span class="num text-primary">').text(p.totalEarnedHours)).append($('<span class="lbl">').text(t('kpiHours'))).appendTo(stats);
      $('<div class="profile-stat-box">').append($('<span class="num text-gold">').text(p.verifiedCertificatesCount)).append($('<span class="lbl">').text(t('kpiCertificates'))).appendTo(stats);

      $('<h5 class="section-title">').text(language === 'ar' ? 'سجل البرامج التدريبية المعتمدة والشهادات' : 'Accredited Programs & Certificates').appendTo(body);
      if (!p.certificates.length) {
        $('<p class="muted">').text(t('noRows')).appendTo(body);
      } else {
        const table = $('<table class="table table-hover align-middle"><thead><tr><th>الدورة</th><th>المحور</th><th>الساعات</th><th>تاريخ الإصدار</th><th>الرقم المرجعي</th><th>تحميل</th></tr></thead><tbody></tbody></table>').appendTo(body);
        const tbody = table.find('tbody');
        p.certificates.forEach(c => {
          const row = $('<tr>').appendTo(tbody);
          $('<td>').text(name(c.courseTitleAr, c.courseTitleEn)).appendTo(row);
          $('<td>').text(name(c.trackNameAr, c.trackNameEn)).appendTo(row);
          $('<td>').text(c.hours).appendTo(row);
          $('<td>').text(new Date(c.issuedAtUtc).toLocaleDateString(language === 'ar' ? 'ar-JO' : 'en-GB')).appendTo(row);
          $('<td>').append($('<code>').text(c.certificateReference)).appendTo(row);
          $('<td>').append($('<a class="btn btn-sm btn-outline-primary" target="_blank">')
            .attr('href', apiBase + '/api/certificates/' + c.id + '/download').text(t('download'))).appendTo(row);
        });
      }
    }).fail(() => body.html('<div class="alert alert-danger">' + t('failed') + '</div>'));
  }

  // ================= Public Verification Logic =================
  function verifyCertificate(reference) {
    const host = $('#verification-result').empty();
    api('/api/certificates/verify?cert=' + encodeURIComponent(reference), 'GET').done(value => {
      const card = $('<div class="lecture-card mt-3">').appendTo(host);
      $('<h4>').addClass(value.status === 0 ? 'text-success' : 'text-danger')
        .text(t(value.status === 0 ? 'certificateValid' : 'certificateRevoked')).appendTo(card);
      $('<p class="mb-1">').text(t('recipient') + ': ' + name(value.recipientNameAr, value.recipientNameEn)).appendTo(card);
      $('<p class="mb-1">').text(t('courseName') + ': ' + name(value.courseTitleAr, value.courseTitleEn)).appendTo(card);
      $('<p class="mb-1">').text(t('instructor') + ': ' + name(value.instructorNameAr, value.instructorNameEn)).appendTo(card);
      $('<p class="mb-1">').text(t('issuedAt') + ': ' + new Date(value.issuedAtUtc).toLocaleDateString(language === 'ar' ? 'ar-JO' : 'en-GB')).appendTo(card);
      if (value.statusReason) {
        $('<p class="text-danger fw-bold mt-2">').text(value.statusReason).appendTo(card);
      }
    }).fail(() => $('<div class="alert alert-warning mt-3">').text(t('certificateNotFound')).appendTo(host));
  }

  $('#open-verification').on('click', () => show('verification'));
  $('#back-to-login').on('click', () => show('login'));
  $('#verification-form').on('submit', function (event) {
    event.preventDefault();
    const reference = $('#verification-reference').val().trim();
    if (reference) verifyCertificate(reference);
  });

  // Forgot Password Form Logic
  $('#open-forgot-password').on('click', () => show('forgot'));
  $('#back-to-login-from-forgot').on('click', () => show('login'));
  $('#forgot-form').on('submit', function (e) {
    e.preventDefault();
    const loginName = $('#forgot-login').val().trim();
    const email = $('#forgot-email').val().trim();
    if (!loginName || !email) return;

    const button = $(this).find('button[type=submit]').prop('disabled', true);
    api('/api/auth/forgot-password', 'POST', { loginName, email })
      .done(() => {
        notice('#status-message', t('passwordSent'), true);
        show('login');
      })
      .fail(xhr => notice('#status-message', xhr.responseJSON?.message || t('failed')))
      .always(() => button.prop('disabled', false));
  });

  // ================= Existing Functions Maintained =================
  function renderHints() {
    const body = $('#hints-body').empty();
    if (!hints.length) { $('<tr>').append($('<td colspan="5">').text(t('noRows'))).appendTo(body); return; }
    hints.forEach(h => {
      const row = $('<tr>');
      $('<td>').text(h.screenKey).appendTo(row);
      $('<td>').text(h.fieldKey).appendTo(row);
      $('<td>').text(h.textAr).appendTo(row);
      $('<td>').text(h.textEn || '—').appendTo(row);
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
      const row = $('<tr>');
      $('<td>').text(item.version).appendTo(row);
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
    table.find('th').eq(0).text(t('employeeNumber'));
    table.find('th').eq(1).text(t('nameAr'));
    table.find('th').eq(2).text(t('rolesLabel'));
    table.find('th').eq(3).text(t('actions'));
    adminUsers.forEach(user => {
      const row = $('<tr>').appendTo(table.find('tbody'));
      $('<td>').text(user.loginName).appendTo(row);
      $('<td>').text(name(user.nameAr, user.nameEn) + (user.isActive ? '' : ' · ' + t('disableUser'))).appendTo(row);
      $('<td>').text((user.roles || []).map(getRoleLabel).join('، ')).appendTo(row);
      $('<td>').append($('<button type="button" class="btn btn-sm btn-outline-primary open-user">').data('id', user.id).text(t('view'))).appendTo(row);
    });
  }

  function renderRoleChecks(selector, selectedKeys) {
    const container = $(selector).empty();
    adminRoles.forEach(r => {
      const item = $('<label class="form-check form-check-inline">').appendTo(container);
      $('<input type="checkbox" class="form-check-input">').val(r.key).prop('checked', selectedKeys.includes(r.key)).appendTo(item);
      $('<span class="form-check-label">').text(name(r.nameAr, r.nameEn)).appendTo(item);
    });
  }

  function renderRoleSelect() {
    const select = $('#role-select'), value = select.val();
    select.empty();
    adminRoles.forEach(r => $('<option>').val(r.key).text(name(r.nameAr, r.nameEn)).appendTo(select));
    if (value) select.val(value);
    if (select.val()) loadRoleMatrix();
  }

  function renderAdminFacultySelect() {
    const select = $('#user-faculty').empty();
    $('<option value="">').text('—').appendTo(select);
    faculties.forEach(f => $('<option>').val(f.id).text(name(f.nameAr, f.nameEn)).appendTo(select));
  }

  function renderNeedFacultySelect() {
    const select = $('#need-faculty').empty();
    faculties.forEach(f => $('<option>').val(f.id).text(name(f.nameAr, f.nameEn)).appendTo(select));
  }

  function openUser(id) {
    selectedUser = adminUsers.find(x => x.id === id) || null;
    if (!selectedUser) return;
    $('#user-form-panel').addClass('d-none');
    $('#user-detail-panel').removeClass('d-none');
    $('#credential-result').addClass('d-none').text('');
    $('#user-detail-name').text(name(selectedUser.nameAr, selectedUser.nameEn));
    $('#user-detail-meta').text(selectedUser.loginName + ' · ' + (selectedUser.email || '—'));
    $('#toggle-user-active').text(t(selectedUser.isActive ? 'disableUser' : 'restoreUser'));
    $('#toggle-user-access').text(t(selectedUser.accessOverrideAllow ? 'removeAccessOverride' : 'allowAccess'));
    renderRoleChecks('#edit-role-list', selectedUser.roles);
    loadUserMatrix();
  }

  function loadRoleMatrix() {
    const key = $('#role-select').val();
    if (!key) return;
    api('/api/admin/roles/' + encodeURIComponent(key) + '/permissions', 'GET').done(rows => {
      const container = $('#role-permission-list').empty();
      const table = $('<table class="table"><thead><tr><th></th><th></th></tr></thead><tbody></tbody></table>').appendTo(container);
      rows.forEach(p => {
        const row = $('<tr>').appendTo(table.find('tbody'));
        $('<td>').text(p.descriptionAr || p.permissionKey).appendTo(row);
        $('<td>').append($('<input type="checkbox" class="form-check-input role-grant">').prop('checked', p.granted).data('key', p.permissionKey)).appendTo(row);
      });
    });
  }

  function loadUserMatrix() {
    if (!selectedUser) return;
    api('/api/admin/users/' + selectedUser.id + '/permissions', 'GET').done(rows => {
      const container = $('#user-permission-list').empty();
      const table = $('<table class="table"><thead><tr><th></th><th></th><th></th></tr></thead><tbody></tbody></table>').appendTo(container);
      table.find('th').eq(0).text(t('screenKey'));
      table.find('th').eq(1).text(t('actions'));
      table.find('th').eq(2).text(t('effective'));
      rows.forEach(p => {
        const row = $('<tr>').appendTo(table.find('tbody'));
        $('<td>').text(p.descriptionAr || p.permissionKey).appendTo(row);
        const cell = $('<td>').appendTo(row);
        const select = $('<select class="form-select user-override">').data('key', p.permissionKey).appendTo(cell);
        [[0, 'inherited'], [1, 'allow'], [2, 'deny']].forEach(([val, lbl]) => $('<option>').val(val).text(t(lbl)).appendTo(select));
        select.val(p.mode);
        $('<td>').append($('<span class="badge ' + (p.effective ? 'text-bg-success' : 'text-bg-secondary') + '">').text(t(p.effective ? 'yes' : 'no'))).appendTo(row);
      });
    });
  }

  function loadSyncData() {
    api('/api/admin/university-sync/runs', 'GET').done(runs => {
      const container = $('#sync-runs').empty();
      const table = $('<table class="table"><thead><tr><th></th><th></th><th></th><th></th></tr></thead><tbody></tbody></table>').appendTo(container);
      runs.forEach(r => {
        const row = $('<tr>').appendTo(table.find('tbody'));
        $('<td>').text(new Date(r.startedAtUtc).toLocaleString()).appendTo(row);
        $('<td>').text(t(r.isSuccess ? 'syncComplete' : 'syncIncomplete')).appendTo(row);
        $('<td>').text(r.addedCount + ' ' + t('added')).appendTo(row);
        $('<td>').text(r.updatedCount + ' ' + t('updated')).appendTo(row);
      });
    });
    api('/api/admin/university-sync/missing-employees', 'GET').done(rows => {
      const container = $('#missing-employees').empty();
      if (!rows.length) { $('<p class="muted">').text(t('noRows')).appendTo(container); return; }
      const table = $('<table class="table"><thead><tr><th></th><th></th></tr></thead><tbody></tbody></table>').appendTo(container);
      rows.forEach(m => {
        const row = $('<tr>').appendTo(table.find('tbody'));
        $('<td>').text(m.employeeId).appendTo(row);
        $('<td>').text(name(m.nameAr, m.nameEn)).appendTo(row);
      });
    });
  }

  function loadEvaluationEditor() {
    api('/api/evaluation/form', 'GET').done(form => {
      $('#evaluation-questions').empty();
      $('#evaluation-current').text(form ? t('version') + ' ' + form.version : t('noRows'));
      (form?.questions?.length ? form.questions : [null]).forEach(addEvaluationQuestion);
    }).fail(xhr => notice('#admin-message', xhr.responseJSON?.message || t('failed')));
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

  function loadAdminCourses() {
    api('/api/admin/courses', 'GET').done(rows => {
      adminCourseRows = rows;
      renderAdminCourses();
    }).fail(xhr => notice('#admin-message', xhr.responseJSON?.message || t('failed')));
  }

  function renderAdminCourses() {
    const host = $('#admin-courses-list').empty();
    if (!adminCourseRows.length) { $('<p class="muted">').text(t('noRows')).appendTo(host); return; }
    const table = $('<table class="table align-middle">').appendTo(host);
    const head = $('<thead><tr></tr></thead>').appendTo(table).find('tr');
    [t('coursesTitle'), t('courseActive'), t('certificates'), t('actions')].forEach(label => $('<th>').text(label).appendTo(head));
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
        $('<a class="btn btn-sm btn-outline-primary ms-2" target="_blank">').attr('href', apiBase + '/api/impact/' + row.id + '/file').text(t('download')).appendTo(line);
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
      $('<a class="btn btn-sm btn-outline-primary ms-2" target="_blank">').attr('href', apiBase + '/api/impact/' + row.id + '/file').text(t('download')).appendTo(line);
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
      versions.forEach(v => $('<option>').val(v.id).text(t('version') + ' ' + v.version).appendTo(select));
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

  function searchDirectory(query, facultyId, select) {
    if (!query || query.trim().length < 2) return;
    api('/api/courses/directory-users?query=' + encodeURIComponent(query.trim()) + (facultyId ? '&facultyId=' + encodeURIComponent(facultyId) : ''), 'GET')
      .done(rows => {
        select.empty();
        $('<option>').val('').text(t('selectPerson')).appendTo(select);
        rows.forEach(x => $('<option>').val(x.id).text(name(x.nameAr, x.nameEn) + ' (' + x.loginName + ')').data('user', x).appendTo(select));
        if (rows.length === 1) {
          select.val(rows[0].id);
          instructorChoice = rows[0];
          $('#course-actual-instructor').val(rows[0].nameAr);
          $('#course-quality-operated').prop('checked', false);
        }
      });
  }

  // Event handlers for dynamic forms and buttons
  $('#login-form').on('submit', function (event) {
    event.preventDefault();
    const loginName = $('#login-name').val().trim(), password = $('#login-password').val();
    rememberMe = $('#remember-me').is(':checked');
    if (!loginName || !password) { notice('#status-message', t('missingLogin')); return; }
    const button = $(this).find('button[type=submit]').prop('disabled', true);
    api('/api/auth/login', 'POST', { loginName, password, rememberMe })
      .done(res => {
        $('#login-password').val('');
        if (res.mustChangePassword) {
          currentAccount = { mustChangePassword: true };
          show('change');
        } else {
          loadAccount();
        }
      })
      .fail(xhr => notice('#status-message', xhr.status === 400 || xhr.status === 401 ? t('invalidLogin') : t('connectionError')))
      .always(() => button.prop('disabled', false));
  });

  $('#change-form').on('submit', function (event) {
    event.preventDefault();
    const oldPassword = $('#old-password').val(), newPassword = $('#new-password').val(), confirmPassword = $('#confirm-password').val();
    if (newPassword !== confirmPassword) { notice('#status-message', t('passwordMismatch')); return; }
    const button = $(this).find('button[type=submit]').prop('disabled', true);
    api('/api/auth/change-password', 'POST', { oldPassword, newPassword, rememberMe })
      .done(() => { $('#change-form')[0].reset(); loadAccount(); })
      .fail(xhr => notice('#status-message', xhr.status === 400 || xhr.status === 401 ? t('changeFailed') : t('connectionError')))
      .always(() => button.prop('disabled', false));
  });

  $('#logout-button').on('click', () => {
    api('/api/auth/logout', 'POST').always(() => {
      signedIn = false;
      currentAccount = null;
      currentCourse = null;
      instructorChoice = null;
      $('#login-name, #login-password').val('');
      $('#header-user-name, #header-user-badge').text('');
      $('#course-details-panel').addClass('d-none');
      $('#user-header-widget, #role-context-nav').addClass('d-none');
      show('login');
    });
  });

  $('#language-toggle').on('click', () => {
    language = language === 'ar' ? 'en' : 'ar';
    translate();
    if (signedIn) {
      api('/api/users/me/language', 'PUT', { language }).fail(() => {});
    }
  });

  // Course actions
  $(document).on('click', '.course-view', function () { showCourseDetails($(this).data('id')); });
  $(document).on('click', '.course-enroll', function () {
    api('/api/courses/' + $(this).data('id') + '/enroll', 'POST').done(() => {
      notice('#course-message', t('enrolled'), true);
      loadCourses();
    }).fail(xhr => notice('#course-message', xhr.responseJSON?.message || t('failed')));
  });

  $('#close-course-details').on('click', () => { $('#course-details-panel').addClass('d-none'); currentCourse = null; });

  $('#course-instructor-search').on('input', function () {
    const facultyId = role('DEAN') && !role('QUALITY_DIRECTOR') && !role('ADMIN') ? currentAccount?.facultyId : $('#course-faculty').val();
    searchDirectory($(this).val(), facultyId, $('#course-instructor-results'));
  });

  $('#course-instructor-results').on('change input', function () {
    const selected = $(this).find(':selected');
    instructorChoice = selected.data('user') || (selected.val() ? { id: selected.val(), nameAr: $('#course-actual-instructor').val().trim() } : null);
    if (instructorChoice) {
      if (instructorChoice.nameAr) $('#course-actual-instructor').val(instructorChoice.nameAr);
      $('#course-quality-operated').prop('checked', false);
    }
  });

  $('#course-quality-operated').on('change', function () {
    if ($(this).is(':checked')) {
      instructorChoice = null;
      $('#course-instructor-results').val('');
    }
  });

  $(document).on('blur', '#course-start, #course-end, #course-deadline, #hist-start, #hist-end', function () {
    const parsed = parseDateTime($(this).val());
    if (parsed) {
      $(this).val(localDateTime(parsed));
    }
  });

  $('#course-form').on('submit', function (e) {
    e.preventDefault();
    const start = parseDateTime($('#course-start').val()), end = parseDateTime($('#course-end').val());
    const deadlineText = $('#course-deadline').val().trim(), deadline = deadlineText ? parseDateTime(deadlineText) : null;
    const qualityOperated = $('#course-quality-operated').is(':checked');

    if (!instructorChoice && !qualityOperated) {
      const selected = $('#course-instructor-results').find(':selected');
      if (selected.data('user')) {
        instructorChoice = selected.data('user');
      } else if (selected.val()) {
        instructorChoice = { id: selected.val(), nameAr: $('#course-actual-instructor').val().trim() };
      }
    }

    if (!$('#course-title-ar').val().trim()) {
      notice('#quality-message', 'يرجى كتابة اسم الدورة بالعربية.');
      $('#course-title-ar').focus();
      return;
    }
    if (!$('#course-objectives-ar').val().trim()) {
      notice('#quality-message', 'يرجى كتابة أهداف الدورة بالعربية.');
      $('#course-objectives-ar').focus();
      return;
    }
    if (!$('#course-audience-ar').val().trim()) {
      notice('#quality-message', 'يرجى كتابة الفئة المستهدفة بالعربية.');
      $('#course-audience-ar').focus();
      return;
    }
    if (!$('#course-actual-instructor').val().trim()) {
      notice('#quality-message', 'يرجى كتابة اسم المحاضر الفعلي بالعربية.');
      $('#course-actual-instructor').focus();
      return;
    }
    if (!start) {
      notice('#quality-message', 'صيغة موعد بداية الدورة غير صحيحة. يمكنك كتابتها مثلاً: 2026-11-20 15:00');
      $('#course-start').focus();
      return;
    }
    if (!end) {
      notice('#quality-message', 'صيغة موعد نهاية الدورة غير صحيحة. يمكنك كتابتها مثلاً: 2026-11-25 15:00');
      $('#course-end').focus();
      return;
    }
    if (new Date(end) <= new Date(start)) {
      notice('#quality-message', 'موعد نهاية الدورة يجب أن يكون بعد موعد البداية.');
      $('#course-end').focus();
      return;
    }
    if (deadlineText && !deadline) {
      notice('#quality-message', 'صيغة موعد نهاية التقييم غير صحيحة. يمكنك كتابتها مثلاً: 2026-12-01 23:59');
      $('#course-deadline').focus();
      return;
    }
    if ((role('ADMIN') || role('QA') || role('QUALITY_DIRECTOR')) && !deadline) {
      notice('#quality-message', 'يرجى تحديد نهاية فترة التقييم للدورة المعتمدة (مثال: 2026-12-01 23:59).');
      $('#course-deadline').focus();
      return;
    }
    if (deadline && new Date(deadline) <= new Date(end)) {
      notice('#quality-message', 'نهاية فترة التقييم يجب أن تكون بعد موعد نهاية الدورة.');
      $('#course-deadline').focus();
      return;
    }
    if (!instructorChoice && !qualityOperated) {
      if (role('DEAN')) {
        notice('#quality-message', t('notEligibleExternal'));
      } else {
        notice('#quality-message', 'يرجى اختيار حساب المحاضر من قائمة البحث أو تفعيل خيار (تدير الجودة الدورة القصيرة نيابة عن المحاضر).');
      }
      return;
    }

    const data = {
      titleAr: $('#course-title-ar').val().trim(),
      titleEn: $('#course-title-en').val().trim() || null,
      trainingTrackId: $('#course-track').val(),
      objectivesAr: $('#course-objectives-ar').val().trim(),
      objectivesEn: $('#course-objectives-en').val().trim() || null,
      targetAudienceAr: $('#course-audience-ar').val().trim(),
      targetAudienceEn: $('#course-audience-en').val().trim() || null,
      deliveryMode: Number($('#course-mode').val()),
      plannedHours: Number($('#course-hours').val()),
      plannedStartUtc: start,
      plannedEndUtc: end,
      facultyId: $('#course-faculty').val() || null,
      instructorUserId: qualityOperated ? null : instructorChoice?.id || null,
      actualInstructorNameAr: $('#course-actual-instructor').val().trim(),
      actualInstructorNameEn: null,
      qualityOperated,
      hasExam: $('#course-has-exam').is(':checked'),
      enrollmentMode: Number($('#course-enrollment').val()),
      evaluationDeadlineUtc: deadline
    };

    const button = $(this).find('button[type=submit]').prop('disabled', true);
    const editId = $('#course-edit-id').val();
    api(editId ? '/api/courses/' + editId + '/resubmit' : '/api/courses', editId ? 'PUT' : 'POST', data)
      .done(() => {
        $('#course-form')[0].reset();
        $('#course-edit-id').val('');
        instructorChoice = null;
        $('[data-quality-tab="proposals"]').click();
        notice('#quality-message', t('courseCreated'), true);
        loadCourses();
      })
      .fail(xhr => notice('#quality-message', xhr.responseJSON?.message || t('failed')))
      .always(() => button.prop('disabled', false));
  });

  $('#cancel-course-form').on('click', () => {
    $('#course-edit-id').val('');
    $('[data-quality-tab="proposals"]').click();
  });

  // Course Details actions
  $('#course-detail-content').on('change', '.evaluation-stats-version', function () {
    showEvaluationStats(currentCourse.course.id, $(this).val(), $('#evaluation-stats .evaluation-stats-results'));
  }).on('submit', '#lecture-form', function (e) {
    e.preventDefault();
    const nameAr = $(this).find('[name=title]').val().trim(), textDate = $(this).find('[name=date]').val().trim();
    const startsAtUtc = textDate ? parseDateTime(textDate) : null;
    if (!nameAr || (textDate && !startsAtUtc)) { notice('#course-detail-message', t('invalidCourse')); return; }
    api('/api/courses/' + currentCourse.course.id + '/lectures', 'POST', { nameAr, nameEn: null, startsAtUtc })
      .done(() => { showCourseDetails(currentCourse.course.id); loadCourses(); })
      .fail(xhr => notice('#course-detail-message', xhr.responseJSON?.message || t('failed')));
  }).on('submit', '#cancellation-form', function (e) {
    e.preventDefault();
    const file = $(this).find('[name=file]')[0].files[0];
    const reason = $(this).find('[name=reason]').val().trim();
    if (!file || !reason) { notice('#course-detail-message', t('invalidCourse')); return; }
    const data = new FormData();
    data.append('reasonAr', reason);
    data.append('file', file);
    multipart('/api/course-cancellations/' + currentCourse.course.id, 'POST', data)
      .done(() => { notice('#course-detail-message', t('saved'), true); loadQualityRequests(); })
      .fail(xhr => notice('#course-detail-message', xhr.responseJSON?.message || t('failed')));
  }).on('click', '.activate-code', function () {
    api('/api/courses/lectures/' + $(this).data('id') + '/activate-code', 'POST').done(result => {
      notice('#course-detail-message', result.attendanceCode + ' · ' + t('codeActive') + ' ' +
        new Date(result.expiresAtUtc).toLocaleTimeString(language === 'ar' ? 'ar-JO' : 'en-GB'), true);
    }).fail(xhr => notice('#course-detail-message', xhr.responseJSON?.message || t('failed')));
  }).on('submit', '.code-form', function (e) {
    e.preventDefault();
    const lectureId = $(this).data('id'), code = $(this).find('.code-input').val().trim();
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
    api('/api/courses/' + currentCourse.course.id + '/end', 'POST')
      .done(() => { showCourseDetails(currentCourse.course.id); loadCourses(); loadMyCertificates(); })
      .fail(xhr => notice('#course-detail-message', xhr.responseJSON?.message || t('failed')));
  }).on('submit', '#exam-form', function (e) {
    e.preventDefault();
    const nameAr = $(this).find('[name=name]').val().trim();
    if (!nameAr) return;
    api('/api/courses/' + currentCourse.course.id + '/exam', 'POST', { nameAr, nameEn: null, startsAtUtc: null })
      .done(() => showCourseDetails(currentCourse.course.id))
      .fail(xhr => notice('#course-detail-message', xhr.responseJSON?.message || t('failed')));
  }).on('change', '.exam-outcome', function () {
    const select = $(this), outcome = Number(select.val());
    if (!outcome) return;
    api('/api/courses/' + currentCourse.course.id + '/exam/results/' + select.data('enrollment'), 'PUT', { outcome })
      .done(() => { showCourseDetails(currentCourse.course.id); loadMyCertificates(); })
      .fail(xhr => notice('#course-detail-message', xhr.responseJSON?.message || t('failed')));
  }).on('input', '#participant-search', function () {
    searchDirectory($(this).val(), null, $('#participant-results'));
  }).on('click', '#add-participant', function () {
    const person = $('#participant-results').val();
    if (!person) { notice('#course-detail-message', t('selectPerson')); return; }
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
      .done(() => { notice('#course-detail-message', t('saved'), true); loadEvaluationForCourse(currentCourse.course.id); loadCertificateGate(currentCourse.course.id); })
      .fail(xhr => notice('#course-detail-message', xhr.responseJSON?.message || t('failed')));
  }).on('submit', '#impact-upload-form', function (event) {
    event.preventDefault();
    const file = $(this).find('[name=file]')[0].files[0];
    if (!file || file.size > 20 * 1024 * 1024 || !/\.(pdf|docx)$/i.test(file.name)) {
      notice('#course-detail-message', t('impactFile')); return;
    }
    const data = new FormData();
    data.append('file', file);
    $.ajax({
      url: apiBase + '/api/impact/courses/' + currentCourse.course.id,
      method: 'POST',
      data,
      processData: false,
      contentType: false,
      xhrFields: { withCredentials: true }
    })
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
    api('/api/courses/' + currentCourse.course.id + '/review', 'POST', { decision: $(this).data('decision'), noteAr })
      .done(() => { showCourseDetails(currentCourse.course.id); loadCourses(); })
      .fail(xhr => notice('#course-detail-message', xhr.responseJSON?.message || t('failed')));
  }).on('click', '.edit-proposal', function () {
    const detail = currentCourse, course = detail.course;
    $('#course-edit-id').val(course.id);
    $('#course-title-ar').val(course.titleAr);
    $('#course-title-en').val(course.titleEn || '');
    $('#course-track').val(course.trainingTrackId);
    $('#course-objectives-ar').val(detail.objectivesAr);
    $('#course-objectives-en').val(detail.objectivesEn || '');
    $('#course-audience-ar').val(detail.targetAudienceAr);
    $('#course-audience-en').val(detail.targetAudienceEn || '');
    $('#course-mode').val(String(detail.deliveryMode));
    $('#course-hours').val(detail.plannedHours);
    $('#course-start').val(localDateTime(detail.plannedStartUtc));
    $('#course-end').val(localDateTime(detail.plannedEndUtc));
    $('#course-faculty').val(course.facultyId || '');
    $('#course-enrollment').val(String(course.enrollmentMode));
    $('#course-actual-instructor').val(course.actualInstructorNameAr);
    instructorChoice = course.instructorUserId ? { id: course.instructorUserId } : null;
    $('#course-has-exam').prop('checked', course.hasExam);
    $('[data-quality-tab="new-proposal"]').click();
  }).on('click', '.approve-course', function () {
    const deadline = parseDateTime($('#approval-deadline').val().trim());
    if (!deadline) { notice('#course-detail-message', t('invalidCourse')); return; }
    api('/api/courses/' + currentCourse.course.id + '/approve', 'POST', {
      evaluationDeadlineUtc: deadline,
      qualityOperated: $('#approval-quality').is(':checked'),
      instructorUserId: currentCourse.course.instructorUserId
    }).done(() => { showCourseDetails(currentCourse.course.id); loadCourses(); })
      .fail(xhr => notice('#course-detail-message', xhr.responseJSON?.message || t('failed')));
  });

  // Admin user & roles events
  $('#user-search-button').on('click', searchAdminUsers);
  $('#user-search').on('keydown', function (e) { if (e.key === 'Enter') { e.preventDefault(); searchAdminUsers(); } });
  $('#user-list').on('click', '.open-user', function () { openUser($(this).data('id')); });
  $('#open-user-form').on('click', function () {
    $('#user-detail-panel').addClass('d-none');
    $('#user-form-panel').removeClass('d-none');
    $('#user-form')[0].reset();
    renderRoleChecks('#create-role-list', []);
  });
  $('#cancel-user-form').on('click', () => $('#user-form-panel').addClass('d-none'));

  $('#user-kind').on('change', function () {
    const external = $(this).val() === 'external';
    $('#external-login-wrap, #external-password-wrap').toggleClass('d-none', !external);
    $('#user-employee-id').closest('.col-md-4').toggleClass('d-none', external);
  });

  $('#user-form').on('submit', function (e) {
    e.preventDefault();
    const external = $('#user-kind').val() === 'external';
    const roleKeys = $('#create-role-list input:checked').map((_, el) => el.value).get();
    const employeeId = Number($('#user-employee-id').val());

    if (!$('#user-name-ar').val().trim() || (!external && employeeId < 1) ||
        (external && (!$('#user-email').val().trim() || !$('#user-external-password').val())) || !roleKeys.length) {
      notice('#admin-message', t('invalidUser')); return;
    }

    const data = {
      employeeId: external ? null : employeeId,
      externalLoginName: external ? $('#user-external-login').val().trim() || null : null,
      nameAr: $('#user-name-ar').val().trim(),
      nameEn: $('#user-name-en').val().trim() || null,
      email: $('#user-email').val().trim() || null,
      isExternalTrainer: external,
      externalPassword: external ? $('#user-external-password').val() : null,
      facultyId: $('#user-faculty').val() || null,
      departmentId: null,
      roleKeys
    };

    const button = $(this).find('button[type=submit]').prop('disabled', true);
    api('/api/admin/users', 'POST', data).done(created => {
      $('#user-form')[0].reset();
      $('#user-form-panel').addClass('d-none');
      notice('#admin-message', t('userCreated') + ' ' + created.loginName, true);
      adminUsers.unshift(created);
      renderAdminUsers();
      openUser(created.id);
    }).fail(xhr => notice('#admin-message', xhr.responseJSON?.message || t('failed')))
      .always(() => button.prop('disabled', false));
  });

  $('#close-user-details').on('click', () => {
    $('#user-detail-panel').addClass('d-none');
    $('#credential-result').addClass('d-none').text('');
    selectedUser = null;
  });

  $('#show-credentials').on('click', function () {
    if (!selectedUser) return;
    api('/api/admin/users/' + selectedUser.id + '/credentials', 'GET').done(value => {
      $('#credential-result').removeClass('d-none').text(t('initialPassword') + ': ' +
        (value.initialPassword || '—') + '\n' + t('currentPassword') + ': ' + value.currentPassword);
    }).fail(xhr => notice('#admin-message', xhr.responseJSON?.message || t('failed')));
  });

  $('#admin-password-form').on('submit', function (e) {
    e.preventDefault();
    if (!selectedUser) return;
    const value = $('#admin-new-password').val();
    api('/api/admin/users/' + selectedUser.id + '/password', 'PUT', { password: value })
      .done(() => {
        $('#admin-new-password').val('');
        $('#credential-result').addClass('d-none').text('');
        notice('#admin-message', t('saved'), true);
      })
      .fail(xhr => notice('#admin-message', xhr.responseJSON?.message || t('failed')));
  });

  $('#toggle-user-active').on('click', function () {
    if (!selectedUser) return;
    if (selectedUser.isActive && !window.confirm(t('confirmDisable'))) return;
    api('/api/admin/users/' + selectedUser.id + '/active', 'PUT', { isActive: !selectedUser.isActive })
      .done(() => {
        selectedUser.isActive = !selectedUser.isActive;
        $('#toggle-user-active').text(t(selectedUser.isActive ? 'disableUser' : 'restoreUser'));
        renderAdminUsers();
        notice('#admin-message', t('saved'), true);
      })
      .fail(xhr => notice('#admin-message', xhr.responseJSON?.message || t('failed')));
  });

  $('#toggle-user-access').on('click', function () {
    if (!selectedUser) return;
    api('/api/admin/users/' + selectedUser.id + '/access-override', 'PUT', { allowed: !selectedUser.accessOverrideAllow })
      .done(() => {
        selectedUser.accessOverrideAllow = !selectedUser.accessOverrideAllow;
        $('#toggle-user-access').text(t(selectedUser.accessOverrideAllow ? 'removeAccessOverride' : 'allowAccess'));
        notice('#admin-message', t('saved'), true);
      })
      .fail(xhr => notice('#admin-message', xhr.responseJSON?.message || t('failed')));
  });

  $('#save-user-roles').on('click', function () {
    if (!selectedUser) return;
    const roleKeys = $('#edit-role-list input:checked').map((_, el) => el.value).get();
    api('/api/admin/users/' + selectedUser.id + '/roles', 'PUT', { roleKeys })
      .done(() => {
        selectedUser.roles = roleKeys;
        renderAdminUsers();
        loadUserMatrix();
        notice('#admin-message', t('saved'), true);
      })
      .fail(xhr => notice('#admin-message', xhr.responseJSON?.message || t('failed')));
  });

  $('#user-permission-list').on('change', '.user-override', function () {
    if (!selectedUser) return;
    const select = $(this);
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
    const password = $('#shared-new-password').val();
    if (!password) return;
    api('/api/admin/shared-initial-password', 'PUT', { password })
      .done(() => {
        $('#shared-new-password').val('');
        $('#shared-result').addClass('d-none').text('');
        notice('#admin-message', t('saved'), true);
      })
      .fail(xhr => notice('#admin-message', xhr.responseJSON?.message || t('failed')));
  });

  $('#role-select').on('change', loadRoleMatrix);
  $('#role-permission-list').on('change', '.role-grant', function () {
    const checkbox = $(this), roleKey = $('#role-select').val();
    api('/api/admin/roles/' + encodeURIComponent(roleKey) + '/permissions/' + encodeURIComponent(checkbox.data('key')),
      'PUT', { granted: checkbox.is(':checked') })
      .done(() => notice('#admin-message', t('saved'), true))
      .fail(xhr => {
        checkbox.prop('checked', !checkbox.is(':checked'));
        notice('#admin-message', xhr.responseJSON?.message || t('failed'));
      });
  });

  $('#refresh-university').on('click', function () {
    const button = $(this).prop('disabled', true);
    notice('#university-result', language === 'ar' ? 'جاري مزامنة بيانات الجامعة من الخوادم المركزية، يرجى الانتظار...' : 'Syncing university data from central servers, please wait...', true);
    api('/api/admin/university-sync', 'POST').done(result => {
      notice('#university-result', t(result.complete ? 'syncComplete' : 'syncIncomplete') + ': ' +
        t('added') + ' ' + result.added + ', ' + t('updated') + ' ' + result.updated, result.complete);
      loadSyncData();
      loadCourses();
    }).fail(xhr => {
      notice('#university-result', xhr.responseJSON?.error || t('failed'));
      loadSyncData();
    }).always(() => button.prop('disabled', false));
  });

  // Dynamic Hint Focus/Blur
  $(document).on('focusin', '[data-hint]', function () {
    const element = this, [screen, field] = $(this).data('hint').split('.');
    $('.field-hint').remove();
    api('/api/settings/hints', 'GET').done(rows => {
      hints = rows;
      if (document.activeElement !== element) return;
      const h = hints.find(x => x.screenKey === screen && x.fieldKey === field);
      if (h) $('<small class="field-hint">').text(language === 'en' && h.textEn ? h.textEn : h.textAr).insertAfter(element);
    });
  }).on('focusout', '[data-hint]', () => $('.field-hint').remove());

  // Policy & Template Forms
  $('#hint-reset').on('click', () => { $('#hint-form')[0].reset(); $('#hint-id').val(''); });
  $('#hint-form').on('submit', function (e) {
    e.preventDefault();
    const id = $('#hint-id').val();
    const data = {
      screenKey: $('#hint-screen').val().trim(),
      fieldKey: $('#hint-field').val().trim(),
      textAr: $('#hint-ar').val().trim(),
      textEn: $('#hint-en').val().trim() || null
    };
    if (!data.screenKey || !data.fieldKey || !data.textAr) { notice('#admin-message', t('invalidHint')); return; }
    const button = $(this).find('button[type=submit]').prop('disabled', true);
    api('/api/settings/hints' + (id ? '/' + id : ''), id ? 'PUT' : 'POST', data)
      .done(() => {
        $('#hint-reset').click();
        notice('#admin-message', t('saved'), true);
        api('/api/settings/hints', 'GET').done(rows => { hints = rows; renderHints(); });
      })
      .fail(xhr => notice('#admin-message', xhr.responseJSON?.message || t('failed')))
      .always(() => button.prop('disabled', false));
  });

  $('#hints-body').on('click', '.edit-hint', function () {
    const h = hints.find(x => x.id === $(this).data('id'));
    if (!h) return;
    $('#hint-id').val(h.id);
    $('#hint-screen').val(h.screenKey);
    $('#hint-field').val(h.fieldKey);
    $('#hint-ar').val(h.textAr);
    $('#hint-en').val(h.textEn || '');
    $('#hint-screen').trigger('focus');
  }).on('click', '.delete-hint', function () {
    if (!window.confirm(t('confirmDelete'))) return;
    api('/api/settings/hints/' + $(this).data('id'), 'DELETE')
      .done(() => {
        notice('#admin-message', t('deleted'), true);
        api('/api/settings/hints', 'GET').done(rows => { hints = rows; renderHints(); });
      })
      .fail(() => notice('#admin-message', t('failed')));
  });

  $('#policy-form').on('submit', function (e) {
    e.preventDefault();
    const min = Number($('#policy-min').val()), max = Number($('#policy-max').val());
    if (min < 8 || max < min || max > 256) { notice('#admin-message', t('invalidPolicy')); return; }
    const data = {
      minimumLength: min,
      maximumLength: max,
      allowSpaces: $('#policy-spaces').is(':checked'),
      requireMixedCharacterClasses: $('#policy-mix').is(':checked'),
      blockCommonPasswords: $('#policy-common').is(':checked')
    };
    api('/api/settings/password-policy', 'PUT', data)
      .done(() => notice('#admin-message', t('saved'), true))
      .fail(xhr => notice('#admin-message', xhr.responseJSON?.message || t('failed')));
  });

  $('#template-form').on('submit', function (e) {
    e.preventDefault();
    const file = $('#template-file')[0].files[0];
    if (!file || file.size > 20 * 1024 * 1024 || !/\.pdf$/i.test(file.name)) { notice('#admin-message', t('invalidPdf')); return; }
    const data = new FormData();
    data.append('nameAr', $('#template-name-ar').val().trim());
    data.append('nameEn', $('#template-name-en').val().trim());
    data.append('file', file);
    const button = $(this).find('button[type=submit]').prop('disabled', true);
    $.ajax({
      url: apiBase + '/api/settings/certificate-templates',
      method: 'POST',
      data,
      processData: false,
      contentType: false,
      xhrFields: { withCredentials: true },
      timeout: 60000
    })
      .done(() => {
        $('#template-form')[0].reset();
        notice('#admin-message', t('uploaded'), true);
        api('/api/settings/certificate-templates', 'GET').done(rows => { templates = rows; renderTemplates(); });
      })
      .fail(xhr => notice('#admin-message', xhr.responseJSON?.message || t('failed')))
      .always(() => button.prop('disabled', false));
  });

  $('#admin-courses-list').on('click', '.disable-admin-course', function () {
    if (!window.confirm(t('courseDeleteWarning'))) return;
    const hideCertificates = $(this).siblings('select').val() === 'true';
    api('/api/admin/courses/' + $(this).data('id'), 'DELETE', { hideCertificates, warningAcknowledged: true })
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

  $('#evaluation-editor').on('submit', function (e) {
    e.preventDefault();
    const questions = $('#evaluation-questions .evaluation-question').map(function () {
      return {
        textAr: $(this).find('.question-ar').val().trim(),
        textEn: $(this).find('.question-en').val().trim() || null,
        options: $(this).find('.evaluation-option').map(function () {
          return { textAr: $(this).find('.option-ar').val().trim(), textEn: $(this).find('.option-en').val().trim() || null };
        }).get()
      };
    }).get();
    api('/api/evaluation/form/publish', 'POST', { questions })
      .done(() => { notice('#admin-message', t('saved'), true); loadEvaluationEditor(); })
      .fail(xhr => notice('#admin-message', xhr.responseJSON?.message || t('failed')));
  });

  // App Initialization
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
      show('verification');
      $('#verification-reference').val(certificateReference);
      verifyCertificate(certificateReference);
    } else {
      loadAccount();
    }
  }).fail(() => {
    show('login');
    notice('#status-message', t('connectionError'));
  });
})();
