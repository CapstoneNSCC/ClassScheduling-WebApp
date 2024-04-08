
const generateCalendar = (element, endpoint) => {
  const calendar = new FullCalendar.Calendar(element, {
    lazyFetching: false,
    initialView: 'timeGridWeek',
    slotMinTime: '08:00',
    slotMaxTime: '18:00',
    slotDuration: '00:30:00',
    slotLabelInterval: '01:00:00',
    height: '720px',
    themeSystem: 'bootstrap5',
    navLinks: false,
    weekends: false,
    allDaySlot: false,
    eventMinHeight: 30,
    headerToolbar: false,
    dayHeaderFormat: { weekday: 'long' },
    nowIndicator: false,
    slotLabelFormat: {
      hour: 'numeric',
      minute: '2-digit',
      omitZeroMinute: true,
      meridiem: 'short'
    },
    eventDisplay: 'block',
    events: endpoint,
    eventContent: (e) => {
      console.log(e.event.startStr);
      let timeOptions = { hour: 'numeric', minute: 'numeric', hour12: true }

      let courseCode = e.event.extendedProps.courseCode;
      let courseName = e.event.extendedProps.courseName;
      let program = e.event.extendedProps.program;
      let startTime = new Date(e.event.startStr);
      let endTime = new Date(e.event.endStr);
      let professor = e.event.extendedProps.professor;
      let classroom = e.event.extendedProps.classroom;


      let courseEl = document.createElement('div');
      courseEl.innerHTML = `${courseCode} - ${courseName}`;

      let programEl = document.createElement('div');
      programEl.innerHTML = program;

      let timeEl = document.createElement('div');
      timeEl.innerHTML = `${startTime.toLocaleString('en-US', timeOptions)} - ${endTime.toLocaleString('en-US', timeOptions)}`;

      let classroomEl = document.createElement('div');
      classroomEl.innerHTML = classroom;

      let professorEl = document.createElement('div');
      professorEl.innerHTML = professor;

      let arrayOfDomNodes = [courseEl, programEl, timeEl, classroomEl, professorEl];
      return { domNodes: arrayOfDomNodes };
    },
    eventMouseEnter: (e) => {
      let popover = bootstrap.Popover.getOrCreateInstance(e.el, {
        title: e.event.extendedProps.courseCode + " " + e.event.extendedProps.courseName,
        content: `
          <div class="test">${e.event.extendedProps.program}</div>
          <div class="test">${e.event.extendedProps.time}</div>
          <div class="test">${e.event.extendedProps.classroom}</div>
          <div class="teacher">${e.event.extendedProps.professor}</div>
        `,
        html: true,
        trigger: 'hover'
      })

      popover.toggle()
    },
  })

  calendar.render()

  return calendar
}