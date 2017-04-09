zuul.constants = {
    api: {
        registerAccount: "/api/UserAccount/Register",
        login: "/api/UserAccount/Login",
        logout: "/api/UserAccount/Logout",
        getThreads: "/api/Forum/Threads",
        getThreadsWithPageNumber: "/api/Forum/Threads/Page/{0}",
        getThreadsSorted: "/api/Forum/Threads/Sort/{0}",
        getThreadsSortedWithPageNumber: "/api/Forum/Threads/Sort/{0}/Page/{1}",
        createThread: "/api/Forum/Thread",
        createReplyToThread: "/api/Forum/Thread/{0}/Reply",
        createReplyToPost: "/api/Forum/Thread/{0}/Post/{1}/Reply",
        getThreadReplies: "/api/Forum/Thread/{0}/Replies"
    },
    pageSize: 25,
    mailtoTemplate: "mailto:{0}",
    sortOrder: {
        date: "Date",
        dateDesc: "DateDesc",
        postedBy : "PostedBy",
        postedByDesc: "PostedByDesc",
        subject: "Subject",
        subjectDesc: "SubjectDesc",
        size: "Size",
        sizeDesc: "SizeDesc"
    },
    defaultSortOrder: "DateDesc"
};