// Parse the sexp definitions and print them in our format
//
// This:
//  (((?* ?x) I remember (?* ?y)) 
//    (Do you often think of ?y)
//    (Does thinking of ?y bring anything else to mind?)
//    (What else do you remember) (Why do you recall ?y right now?)
//    (What in the present situation reminds you of ?y)
//    (What is the connection between me and ?y))
//
// Well become:
//   new LookupPattern(
//    @"(.*) I remember (.*)",
//    @"Do you often think of \2",
//    @"Does thinking of \2 bring anything else to mind?",
//    @"What else do you remember",
//    @"Why do you recall \2 right now?",
//    @"What in the present situation reminds you of \2",
//    @"What is the connection between me and \2"),
 
