
document.getElementById('logoFile').addEventListener('change', function (e) {
    const file = e.target.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function (event) {
            document.getElementById('logoPreview').src = event.target.result;
            document.getElementById('logoPreview').style.display = 'block';
        };
        reader.readAsDataURL(file);
    }
});

document.querySelectorAll('.color-input').forEach(input => {
    input.addEventListener('input', function () {
        const color = this.value;
        const previewElement = document.querySelector(`[data-preview-for="${this.id}"]`);
        if (previewElement) {
            previewElement.style.backgroundColor = color;
        }
    });
});