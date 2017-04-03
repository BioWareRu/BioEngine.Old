$(function () {
    window.bwFingerprint = null;
    window.getUserFingerPrint = function (callback) {
        if (window.bwFingerprint == null) {
            new Fingerprint2().get(function (result, components) {
                window.bwFingerprint = result;
                callback(window.bwFingerprint)
            });
        } else {
            callback(window.bwFingerprint)
        }
    }
});