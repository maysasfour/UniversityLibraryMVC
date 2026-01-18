document.addEventListener('DOMContentLoaded', function () {
 
    const xmlFileInputs = document.querySelectorAll('input[type="file"][accept=".xml"]');
    xmlFileInputs.forEach(input => {
        input.addEventListener('change', function (e) {
            const file = e.target.files[0];
            if (file) {
                const fileName = file.name.toLowerCase();
                const isValid = fileName.endsWith('.xml');

                if (!isValid) {
                    alert('Please select a valid XML file (.xml extension)');
                    this.value = '';
                }
            }
        });
    });

    const copySchemaBtn = document.querySelector('[onclick*="copySchema"]');
    if (copySchemaBtn) {
        copySchemaBtn.addEventListener('click', function () {
            const schemaTextarea = document.querySelector('textarea[readonly]');
            if (schemaTextarea) {
                navigator.clipboard.writeText(schemaTextarea.value).then(() => {
                    alert('XML Schema copied to clipboard!');
                }).catch(err => {
                    console.error('Failed to copy schema:', err);
                });
            }
        });
    });

document.querySelectorAll('input[type="file"]').forEach(input => {
    input.addEventListener('change', function () {
        const file = this.files[0];
        if (file && file.size > 2 * 1024 * 1024) {
            alert('File size exceeds 2MB limit. Please select a smaller file.');
            this.value = '';
        }
    });
});
});