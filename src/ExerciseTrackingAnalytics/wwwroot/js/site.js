/**
 * Activities Sync Modal
 * Corresponding HTML is in partial `_StravaActivitySyncModalPartial.cshtml`
 */
class StravaActivitiesSyncModal {
    constructor(syncCompleteOkButtonCallback = () => { }, noButtonCallback = () => { }) {
        this.stravaSyncCheckCookieName = 'last-strava-sync-check';

        this.syncCompleteOkButtonCallback = syncCompleteOkButtonCallback ?? (() => { });
        this.noButtonCallback = noButtonCallback ?? (() => { });

        this.rootElement = document.getElementById('activities_sync_modal');
        this.bodyElement = document.getElementById('activities_sync_modal--body');
        this.yesNoButtonsContainer = document.getElementById('activities_sync_modal--yes_no_buttons_container');
        this.okButtonContainer = document.getElementById('activities_sync_modal--ok_button_container');
        this.yesButton = document.getElementById('activities_sync_modal--yes_button');
        this.noButton = document.getElementById('activities_sync_modal--no_button');
        this.okButton = document.getElementById('activities_sync_modal--ok_button');

        this.yesButton.addEventListener('click', (e) => {
            e.preventDefault();
            Cookies.set(this.stravaSyncCheckCookieName, new Date().toJSON());
            console.log('Exercise Tracking Analytics: Syncing recent Strava activities...');
            this.bodyElement.innerText = 'Checking for recent activities...';
            this.yesButton.disabled = true;
            this.noButton.disabled = true;

            fetch('/api/strava-activity-sync', {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            })
                .then(async (response) => {
                    console.log(`Exercise Tracking Analytics: Recent Activities Sync API Call Response Status: ${response.status} ${response.statusText}`);
                    const jsonResult = await response.json();
                    console.log('Exercise Tracking Analytics: Recent Activities Sync API Call Response JSON Data:', jsonResult);

                    if (response.ok) {
                        this.bodyElement.innerText = (jsonResult && jsonResult.message) ?
                            jsonResult.message :
                            'Recent activity check complete.';
                    } else {
                        this.bodyElement.innerText = (jsonResult && jsonResult.message) ?
                            jsonResult.message :
                            'We were unable to sync recent Strava Activities due to an unexpected system error.';
                    }

                    this.yesNoButtonsContainer.style.display = 'none';
                    this.okButtonContainer.style.display = 'block';
                });
        });

        this.noButton.addEventListener('click', (e) => {
            e.preventDefault();
            Cookies.set(this.stravaSyncCheckCookieName, new Date().toJSON());

            if (typeof (this.noButtonCallback) === 'function') {
                this.noButtonCallback();
            }
        });

        this.okButton.addEventListener('click', (e) => {
            e.preventDefault();

            if (typeof (this.syncCompleteOkButtonCallback) === 'function') {
                this.syncCompleteOkButtonCallback();
            }
        });
    }

    getLastCheckCookie() {
        return Cookies.get(this.stravaSyncCheckCookieName);
    }

    hasLastCheckCookie() {
        return !!(Cookies.get(this.stravaSyncCheckCookieName));
    }

    open() {
        const bootstrapModal = new bootstrap.Modal(this.rootElement);
        bootstrapModal.show();
    }
}
