/*!
 * FileInput Portuguese Translations
 *
 * This file must be loaded after 'fileinput.js'. Patterns in braces '{}', or
 * any HTML markup tags in the messages must not be converted or translated.
 *
 * @see http://github.com/kartik-v/bootstrap-fileinput
 *
 * NOTE: this file must be saved in UTF-8 encoding.
 */
(function ($) {
    "use strict";

    $.fn.fileinputLocales['pt'] = {
        fileSingle: 'ficheiro',
        filePlural: 'ficheiros',
        browseLabel: 'Procurar &hellip;',
        removeLabel: 'Remover',
        removeTitle: 'Remover ficheiros seleccionados',
        cancelLabel: 'Cancelar',
        cancelTitle: 'Abortar carregamento ',
        pauseLabel: 'Pause',
        pauseTitle: 'Pause ongoing upload',
        uploadLabel: 'Carregar',
        uploadTitle: 'Carregar ficheiros seleccionados',
        msgNo: 'Não',
        msgNoFilesSelected: '',
        msgPaused: 'Paused',
        msgCancelled: 'Cancelado',
        msgPlaceholder: 'Select {files}...',
        msgZoomModalHeading: 'Pré-visualização detalhada',
        msgFileRequired: 'You must select a file to upload.',
        msgSizeTooSmall: 'File "{name}" (<b>{size} KB</b>) is too small and must be larger than <b>{minSize} KB</b>.',
        msgSizeTooLarge: 'Ficheiro "{name}" (<b>{size} KB</b>) excede o tamanho máximo permido de <b>{maxSize} KB</b>.',
        msgFilesTooLess: 'Deve seleccionar pelo menos <b>{n}</b> {files} para fazer upload.',
        msgFilesTooMany: 'Número máximo de ficheiros seleccionados <b>({n})</b> excede o limite máximo de <b>{m}</b>.',
        msgFileNotFound: 'Ficheiro "{name}" não encontrado!',
        msgFileSecured: 'Restrições de segurança preventem a leitura do ficheiro "{name}".',
        msgFileNotReadable: 'Ficheiro "{name}" não pode ser lido.',
        msgFilePreviewAborted: 'Pré-visualização abortado para o ficheiro "{name}".',
        msgFilePreviewError: 'Ocorreu um erro ao ler o ficheiro "{name}".',
        msgInvalidFileName: 'Invalid or unsupported characters in file name "{name}".',
        msgInvalidFileType: 'Tipo inválido para o ficheiro "{name}". Apenas ficheiros "{types}" são suportados.',
        msgInvalidFileExtension: 'Extensão inválida para o ficheiro "{name}". Apenas ficheiros "{extensions}" são suportados.',
        msgFileTypes: {
            'image': 'image',
            'html': 'HTML',
            'text': 'text',
            'video': 'video',
            'audio': 'audio',
            'flash': 'flash',
            'pdf': 'PDF',
            'object': 'object'
        },
        msgUploadAborted: 'O upload do arquivo foi abortada',
        msgUploadThreshold: 'Processing...',
        msgUploadBegin: 'Initializing...',
        msgUploadEnd: 'Done',
        msgUploadResume: 'Resuming upload...',
        msgUploadEmpty: 'No valid data available for upload.',
        msgUploadError: 'Upload Error',
        msgDeleteError: 'Delete Error',
        msgProgressError: 'Error',
        msgValidationError: 'Erro de validação',
        msgLoading: 'A carregar ficheiro {index} de {files} &hellip;',
        msgProgress: 'A carregar ficheiro {index} de {files} - {name} - {percent}% completo.',
        msgSelected: '{n} {files} seleccionados',
        msgFoldersNotAllowed: 'Arrastar e largar ficheiros apenas! {n} pasta(s) ignoradas.',
        msgImageWidthSmall: 'Largura do arquivo de imagem "{name}" deve ser pelo menos {size} px.',
        msgImageHeightSmall: 'Altura do arquivo de imagem "{name}" deve ser pelo menos {size} px.',
        msgImageWidthLarge: 'Largura do arquivo de imagem "{name}" não pode exceder {size} px.',
        msgImageHeightLarge: 'Altura do arquivo de imagem "{name}" não pode exceder {size} px.',
        msgImageResizeError: 'Could not get the image dimensions to resize.',
        msgImageResizeException: 'Erro ao redimensionar a imagem.<pre>{errors}</pre>',
        msgAjaxError: 'Something went wrong with the {operation} operation. Please try again later!',
        msgAjaxProgressError: '{operation} failed',
        msgDuplicateFile: 'File "{name}" of same size "{size} KB" has already been selected earlier. Skipping duplicate selection.',
        msgResumableUploadRetriesExceeded:  'Upload aborted beyond <b>{max}</b> retries for file <b>{file}</b>! Error Details: <pre>{error}</pre>',
        msgPendingTime: '{time} remaining',
        msgCalculatingTime: 'calculating time remaining',
        ajaxOperations: {
            deleteThumb: 'file delete',
            uploadThumb: 'file upload',
            uploadBatch: 'batch file upload',
            uploadExtra: 'form data upload'
        },
        dropZoneTitle: 'Arrastar e largar ficheiros aqui &hellip;',
        dropZoneClickTitle: '<br>(or click to select {files})',
        fileActionSettings: {
            removeTitle: 'Remover arquivo',
            uploadTitle: 'Carregar arquivo',
            uploadRetryTitle: 'Retry upload',
            downloadTitle: 'Download file',
            zoomTitle: 'Ver detalhes',
            dragTitle: 'Move / Rearrange',
            indicatorNewTitle: 'Ainda não carregou',
            indicatorSuccessTitle: 'Carregado',
            indicatorErrorTitle: 'Carregar Erro',
            indicatorPausedTitle: 'Upload Paused',
            indicatorLoadingTitle:  'A carregar ...'
        },
        previewZoomButtonTitles: {
            prev: 'View previous file',
            next: 'View next file',
            toggleheader: 'Toggle header',
            fullscreen: 'Toggle full screen',
            borderless: 'Toggle borderless mode',
            close: 'Close detailed preview'
        }
    };
})(window.jQuery);