document.addEventListener('DOMContentLoaded', function () {
    const storedTheme = localStorage.getItem('library-theme');
    const prefersDark = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
    const theme = storedTheme || (prefersDark ? 'dark' : 'light');
    document.documentElement.setAttribute('data-theme', theme);

    const themeToggle = document.getElementById('themeToggle');
    const updateThemeToggle = () => {
        if (!themeToggle) return;
        const isDark = document.documentElement.getAttribute('data-theme') === 'dark';
        themeToggle.innerHTML = isDark
            ? '<i class="bi bi-sun"></i><span>Light</span>'
            : '<i class="bi bi-moon-stars"></i><span>Dark</span>';
    };

    updateThemeToggle();

    themeToggle?.addEventListener('click', () => {
        const nextTheme = document.documentElement.getAttribute('data-theme') === 'dark' ? 'light' : 'dark';
        document.documentElement.setAttribute('data-theme', nextTheme);
        localStorage.setItem('library-theme', nextTheme);
        updateThemeToggle();
    });

    const cursorDot = document.createElement('span');
    cursorDot.className = 'cursor-dot';
    document.body.appendChild(cursorDot);

    document.addEventListener('pointermove', event => {
        cursorDot.style.transform = `translate(${event.clientX}px, ${event.clientY}px)`;
    });

    const forms = document.querySelectorAll('form');
    forms.forEach(form => {
        form.addEventListener('submit', function (e) {
            let isValid = true;

            const requiredFields = this.querySelectorAll('[required]');
            requiredFields.forEach(field => {
                if (!field.value.trim()) {
                    isValid = false;
                    const errorElement = field.nextElementSibling;
                    if (errorElement && errorElement.classList.contains('text-danger')) {
                        errorElement.style.display = 'block';
                    }
                    field.classList.add('is-invalid');
                }
            });

            if (!isValid) {
                e.preventDefault();
                e.stopPropagation();
                return false;
            }
        });
    });

    const searchInputs = document.querySelectorAll('.form-control[placeholder*="Search"]');
    searchInputs.forEach(input => {
        input.addEventListener('keyup', function () {
            const searchTerm = this.value.toLowerCase();
            const tableRows = this.closest('.card-body')?.querySelectorAll('table tbody tr');

            if (tableRows) {
                tableRows.forEach(row => {
                    const text = row.textContent.toLowerCase();
                    row.style.display = text.includes(searchTerm) ? '' : 'none';
                });
            }
        });
    });

    const tabLinks = document.querySelectorAll('.nav-link[data-bs-toggle="tab"]');
    tabLinks.forEach(link => {
        link.addEventListener('click', function (e) {
            e.preventDefault();
            const target = this.getAttribute('href');
            document.querySelectorAll('.tab-pane').forEach(pane => {
                pane.classList.remove('show', 'active');
            });
            document.querySelector(target).classList.add('show', 'active');
        });
    });

    const alerts = document.querySelectorAll('.alert-dismissible');
    alerts.forEach(alert => {
        setTimeout(() => {
            alert.classList.add('fade');
            setTimeout(() => {
                alert.style.display = 'none';
            }, 150);
        }, 5000);
    });
});
