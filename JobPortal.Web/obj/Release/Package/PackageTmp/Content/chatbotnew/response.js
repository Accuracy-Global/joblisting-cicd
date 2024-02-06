function getresponse(userInput) {
    if (userInput == "hi") {
        return "hi! how can I help you?";
    }
    else if (userInput.includes("ok")) {
        return "It was nice chatting with you. Just text me. If you need something, please let me know."
    }
    else if (userInput.includes("")) {
        return "Oops! Sorry, I didn't understand your question. Please rephrase it.";
    }
    else {
        return "Please first select one from above action";
    }

}
// For Students and Interns
function getresponse1(userInput) {
    if (userInput == "hi") {
        return "hi! how can I help you?";
    }
    else if (userInput == "Student") {
        return "As students, you can submit your questions";
    }
    else if (userInput == "Interns") {
        return "As interns, you can submit your questions";
    }
    else if (userInput == "hello") {
        return "hello ! how are you?";
    }
    else if (userInput.includes("register")) {
        return "Click on the Register button and fill in the details to get registered on our job listing portal.";
    }
    else if (userInput.includes("part-time") || userInput.includes("part time") || userInput.includes("part time job")) {
        return "You have to complete the registration process and build your resume online.";
    }
    else if (userInput.includes("jobs") || userInput.includes("job") || userInput.includes("latest") || userInput.includes("latest jobs")) {
        return "Click on Search Jobs or the Home page.";
    }
    else if (userInput.includes("campus") || userInput.includes("selection")) {
        return "You have to register and build your resume online.";
    }
    else if (userInput.includes("company") || userInput.includes("located")) {
        return "We are headquartered in Singapore.";
    }
    else if (userInput.includes("someone") || userInput.includes("other") || userInput.includes("someone name")) {
        return "You have to click on search people and then search by name.";
    }
    else if (userInput.includes("available") || userInput.includes("jobs available")) {
        return "We have Part-time, Full time, Fresher and Internship jobs vacancy listings.";
    }
    else if (userInput.includes("apply")) {
        return "First you need to Search for a Job,  then Apply for Jobs, and finally Get Your Job";
    }
    else if (userInput.includes("taking")) {
        return "We have active listing of jobs that are posted by the companies and recruiters.";
    }
    else if (userInput.includes("matches") || userInput.includes("skills")) {
        return "Go to the home page and click on the register button.";
    }
    else if (userInput.includes("experienced")) {
        return "Click on search job and see the view.";
    }
    else if (userInput.includes("kinds") || userInput.includes("kind")) {
        return "You can see all types of jobs as per your requirements.";
    }
    else if (userInput.includes("pursuing")) {
        return "Well, some specific types of job may allow you to pursue your education along with the job. Please read the job description to understand this. ";
    }
    else if (userInput.includes("full-time") || userInput.includes("full time")) {
        return "You have to complete the registration process and build your resume online.";
    }
    else if (userInput.includes("internship")) {
        return "You have to complete the registration process and build your resume online.";
    }
    else if (userInput.includes("searching")) {
        return "Remote-based jobs or work from the office";
    }
    else if (userInput.includes("thank") || userInput.includes("thanks")) {
        return "It was nice chatting with you. Just text me.If you need something, please let me know."
    }
    else if (userInput.includes("ok")) {
        return "It was nice chatting with you. Just text me If you need something, please let me know."
    }
    else if (userInput.includes("")) {
        return "Sorry, I didn't catch the question, please rephrase it";
    }
}
// For JobSeekers
function getresponse3(userInput) {
    if (userInput == "hi") {
        return "hi! how can I help you?";
    }
    else if (userInput == "jobseeker" || userInput == "jobseekers" || userInput == "Jobseeker" || userInput == "Jobseekers") {
        return "As jobseeker, you can submit your questions";
    }

    else if (userInput == "hey") {
        return "hey! how are you ?";
    }
    else if (userInput == "hello") {
        return "hello ! how are you?";
    }
    else if (userInput.includes("registered")) {
        return ": Click on Search Jobs and you will find the list of companies that are registered on our job portal.";
    }
    else if (userInput.includes("recommended")) {
        return "You have to complete the registration process first. Build your resume online and check it on social pages.( Job Recommendation field should be their separately )";
    }
    else if (userInput.includes("detail")) {
        return "Every job detail and job description is given along with company details and required Skills, go through them and apply.";
    }

    else if (userInput.includes("register")) {
        return "Click on the Register button.  Once you land on the job listing page click on the register button to start your job search.";
    }
    else if (userInput.includes("jobs") || userInput.includes("job") || userInput.includes("latest") || userInput.includes("latest jobs")) {
        return "Click on Search Jobs or the Home page.";
    }
    else if (userInput.includes("part-time") || userInput.includes("part time") || userInput.includes("part time job")) {
        return "You have to complete the registration process and build your resume online.";
    }
    else if (userInput.includes("campus") || userInput.includes("selection")) {
        return "You have to complete the registration process to get information related to campus selection.";
    }
    else if (userInput.includes("company") || userInput.includes("located")) {
        return "We are headquartered in Singapore.";
    }
    else if (userInput.includes("language")) {
        return "In-Home Page Select the language option.";
    }
    else if (userInput.includes("change") || userInput.includes("password")) {
        return "Login to your account, click on settings, and click on Change Password.";
    }
    else if (userInput.includes("download") || userInput.includes("apps")) {
        return "In the play store or app store, search for the app and then download it.";
    }
    else if (userInput.includes("update") || userInput.includes("resume")) {
        return "Login to your account, click on settings, and then click on My Profiles.";
    }
    else if (userInput.includes("someone") || userInput.includes("other") || userInput.includes("someone name")) {
        return "You have to click on search people and then search by name.";
    }
    else if (userInput.includes("available") || userInput.includes("jobs available")) {
        return "We have Part-time, Full time, Fresher and Internship jobs listed on our portal.";
    }
    else if (userInput.includes("apply")) {
        return "First you need to register and then search for the type of job that you want to apply.";
    }
    else if (userInput.includes("taking")) {
        return "We have active listing of jobs that are posted by the companies and recruiters.";
    }
    else if (userInput.includes("matches") || userInput.includes("skills")) {
        return "You need to register an account to search for jobs. Start searching for job listings that are relevant to your skills and qualifications on our website.";
    }
    else if (userInput.includes("experienced")) {
        return "Click on 'search job' and see the view.";
    }
    else if (userInput.includes("kinds") || userInput.includes("kind")) {
        return "All types of jobs as per your requirements.";
    }
    else if (userInput.includes("pursuing")) {
        return "Well, some specific types of job (Internship & Part-time) may allow you to pursue your education along with the job. Please read the job description to understand this.";
    }
    else if (userInput.includes("full-time") || userInput.includes("full time")) {
        return "You have to complete the registration process and build your resume online.  Then you can search the portal for available full-time jobs.";
    }
    else if (userInput.includes("internship")) {
        return "You have to complete the registration process and build your resume online. Then you can search the portal for available internship jobs.";
    }
    else if (userInput.includes("searching")) {
        return "Remote-based jobs or work from the office";
    }
    else if (userInput.includes("specific") || userInput.includes("role") || userInput.includes("serve")) {
        return "This is a good question to ask an interviewer.";
    }
    else if (userInput.includes("thank") || userInput.includes("thanks")) {
        return "It was nice chatting with you. Just text me.If you need something, please let me know."
    }
    else if (userInput.includes("ok")) {
        return "It was nice chatting with you. Just text me If you need something, please let me know."
    }
    else if (userInput.includes("")) {
        return "Sorry, I didn't catch the question, please rephrase it";
    }
}
function getresponse4(userInput) {
    if (userInput == "hi") {
        return "hi! how can I help you?";
    }
    else if (userInput == "Employers" || userInput == "Employer" || userInput == "employers" || userInput == "employer") {
        return "As Employers, you can submit your questions";
    }
    else if (userInput == "hey") {
        return "hey! how are you ?";
    }
    else if (userInput == "hello") {
        return "hello ! how are you?";
    }
    else if (userInput.includes("register my company")) {
        return "Yes, you have to complete the registration process and build your company profile.";
    }
    else if (userInput.includes("register")) {
        return "Click on the Register button.";
    }
    else if (userInput.includes("international job")) {
        return "Log in with your company credentials, click on My Work, and then select Listed Jobs.";
    }
    else if (userInput.includes("new jobs") || userInput.includes("new job") || userInput.includes("list") || userInput.includes("post")) {
        return "Log in with your company credentials, click on My Work, and then select List Jobs.";
    }

    else if (userInput.includes("upload") || userInput.includes("company's logo") || userInput.includes("company logo")) {
        return "Yes you have to upload your business logo.";
    }
    else if (userInput.includes("Search for Jobseekers ")) {
        return "Click on Search Jobseekers or log in to your account and search Jobseekers.";
    }

    else if (userInput.includes("Jobseekers")) {
        return "Click on Search Jobseekers or log in to your account and search Jobseekers.";
    }
    else if (userInput.includes("expired")) {
        return "Please register an account and find the job you want";
    }
    else if (userInput.includes("experienced")) {
        return "Click on 'search job' and see the view.";
    }
    else if (userInput.includes("postings") || userInput.includes("resposnses")) {
        return "Log in with your company credentials, click on My Work, and then select Listed Jobs.";
    }
    else if (userInput.includes("scheduled interviews") || userInput.includes("interviews")) {
        return "Log in with your company credentials and click Interviews.(Need to set calendar for interview schedules and reminder alert and automatic invite link should go to candidates from portal.) ";
    }
    else if (userInput.includes("users") || userInput.includes("manage") || userInput.includes("add")) {
        return "Log in with company credentials and, in Social Page below on the left, invite friends to be there.";
    }
    else if (userInput.includes("grievances") || userInput.includes("feedback")) {
        return "Log in with your company credentials and click Campaign.";
    }
    else if (userInput.includes("events") || userInput.includes("fairs")) {
        return "On the home page you can find news on current job fairs and events.";
    }
    else if (userInput.includes("announcements")) {
        return "Log in with your company credentials and click Campaign.";
    }
    else if (userInput.includes("feedback")) {
        return "Log in with your company credentials and click Feedback.";
    }
    else if (userInput.includes("thank") || userInput.includes("thanks")) {
        return "It was nice chatting with you. Just text me.If you need something, please let me know."
    }
    else if (userInput.includes("ok")) {
        return "It was nice chatting with you. Just text me If you need something, please let me know."
    }
    else if (userInput.includes("")) {
        return "Sorry, I didn't catch the question, please rephrase it";
    }
}
// for 
function getresponse5(userInput) {
    if (userInput == "hi") {
        return "hi! how can I help you?";
    }
    else if (userInput == "Recruitment Agency") {
        return "As Recruitment Agency, you can submit your questions";
    }

    else if (userInput == "hey") {
        return "hey! how are you ?";
    }
    else if (userInput == "hello") {
        return "hello ! how are you?";
    }
    else if (userInput.includes("register my agency") || userInput.includes("recruitment")) {
        return "Yes, you have to complete the registration process and build your recruitment agency  profile online.";
    }
    else if (userInput.includes("register")) {
        return "Click on the Register button.";
    }
    else if (userInput.includes("jobs") || userInput.includes("job") || userInput.includes("list") || userInput.includes("post")) {
        return " Log in with your recruitment agency credentials, click on My Work, and then select List Jobs.";
    }


    else if (userInput.includes("logo") || userInput.includes("agency's") || userInput.includes("agency logo")) {
        return "Yes";
    }
    else if (userInput.includes("post new jobs") || userInput.includes("jobs") || userInput.includes("post")) {
        return "Log in with your recruitment agency credentials, click on My Work, and then select Listed Jobs.";
    }
    else if (userInput.includes("international")) {
        return "Log in with your recruitment agency credentials, click on My Work, and then select Listed Jobs.";
    }
    else if (userInput.includes("Jobseekers")) {
        return "Click on Search Jobseekers or log in to your account and search Jobseekers.";
    }
    else if (userInput.includes("expired")) {
        return "Please register an account and find the job you want";
    }
    else if (userInput.includes("experienced")) {
        return "Click on 'search job' and see the view.";
    }
    else if (userInput.includes("postings") || userInput.includes("resposnses")) {
        return "Log in with your recruitment agency credentials, click on My Work, and then select Listed Jobs.";
    }
    else if (userInput.includes("interviews")) {
        return "Log in with your recruitment agency credentials and click Interviews.";
    }
    else if (userInput.includes("users") || userInput.includes("manage") || userInput.includes("add")) {
        return "Log in with company credentials and, in Social Page below on the left, invite friends to be there.";
    }
    else if (userInput.includes("grievances") || userInput.includes("feedback")) {
        return "Log in with your recruitment agency credentials and click Campaign.";
    }
    else if (userInput.includes("events") || userInput.includes("fairs")) {
        return "On the home page";
    }
    else if (userInput.includes("announcements")) {
        return "Log in with your recruitment agency credentials and click Campaign.";
    }
    else if (userInput.includes("successful hiring") || userInput.includes("process")) {
        return "Shortlisting the right candidates falling into all the criteria for the positions";
    }
    else if (userInput.includes("hiring")) {
        return "Quality Data -to maintain quality standards, we have double layered quality check system so that not even a single profile is forwarded which is out of the criteria ";
    }
    else if (userInput.includes("feedback")) {
        return "Log in with your recruitment agency credentials and click Feedback.";
    }
    else if (userInput.includes("reports") || userInput.includes("managers")) {
        return "Interest checking of the candidates for the positions ";
    }
    else if (userInput.includes("sources") || userInput.includes("methods")) {
        return "Quality cross-checking of the candidates & Coordination with candidates for the interview.";
    }

    else if (userInput.includes("thank") || userInput.includes("thanks")) {
        return "It was nice chatting with you. Just text me.If you need something, please let me know.";
    }
    else if (userInput.includes("ok")) {
        return "It was nice chatting with you. Just text me If you need something, please let me know.";
    }
    else if (userInput.includes("")) {
        return "Sorry, I didn't catch the question, please rephrase it";
    }
}
function getresponse6(userInput) {
    if (userInput == "hi") {
        return "hi! how can I help you?";
    }
    else if (userInput == "Institutes") {
        return "As Institutes, you can submit your questions";
    }

    else if (userInput == "hey") {
        return "hey! how are you ?";
    }
    else if (userInput == "hello") {
        return "hello ! how are you?";
    }
    else {
        return "We are working on it";
    }
}
function getresponse7(userInput) {
    if (userInput == "hi") {
        return "hi! how can I help you?";
    }
    else if (userInput == "Freelancers") {
        return "As Freelancers, you can submit your questions";
    }

    else if (userInput == "hey") {
        return "hey! how are you ?";
    }
    else if (userInput == "hello") {
        return "hello ! how are you?";
    }
    else {
        return "We are working on it";
    }
}
function getresponse8(userInput) {
    if (userInput == "hi") {
        return "hi! how can I help you?";
    }
    else if (userInput == "Partners") {
        return "As Partners, you can submit your questions";
    }

    else if (userInput == "hey") {
        return "hey! how are you ?";
    }
    else if (userInput == "hello") {
        return "hello ! how are you?";
    }
    else {
        return "We are working on it";
    }
}