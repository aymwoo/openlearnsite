function switchProfileTab(url, tabId) {
                    document.getElementById('modernGroupModalIframe').src = url;
                    document.querySelectorAll('.profile-tab').forEach(function(tab) {
                        tab.classList.remove('active');
                    });
                    var activeTab = document.getElementById(tabId);
                    if (activeTab) {
                        activeTab.classList.add('active');
                    }
                }

                function openModernGroupModal() {
                    var modal = document.getElementById('modernGroupModal');
                    var backdrop = document.getElementById('modernGroupModalBackdrop');
                    var panel = document.getElementById('modernGroupModalPanel');
                    var iframe = document.getElementById('modernGroupModalIframe');

                    // Populate student info bar from sidebar controls
                    (function() {
                        var avatarSrc = (document.getElementById(window.__myinfoConfig.imagefaceId) || {}).src || '';
                        var name = (document.getElementById(window.__myinfoConfig.snameId) || {}).innerText || '';
                        var num = (document.getElementById(window.__myinfoConfig.snumId) || {}).innerText || '';
                        var cls = (document.getElementById(window.__myinfoConfig.sclassId) || {}).innerText || '';
                        var rankEl = document.getElementById(window.__myinfoConfig.labelRankId);
                        var rankHtml = rankEl ? rankEl.innerHTML : '';
                        // Extract plain text rank label from HTML (strip img tags, keep text nodes)
                        var tmp = document.createElement('div');
                        tmp.innerHTML = rankHtml;
                        // Try to find an img title/alt, or just grab non-empty text
                        var rankText = '';
                        var imgs = tmp.querySelectorAll('img');
                        if (imgs.length > 0) {
                            rankText = imgs[0].title || imgs[0].alt || '';
                        }
                        if (!rankText) {
                            rankText = (tmp.textContent || '').replace(/\s+/g,' ').trim().split(' ')[0];
                        }

                        var barAvatar = document.getElementById('modalStudentAvatar');
                        var barName = document.getElementById('modalStudentName');
                        var barRank = document.getElementById('modalStudentRankBadge');
                        var barNum = document.getElementById('modalStudentNum');
                        var barClass = document.getElementById('modalStudentClass');

                        if (barAvatar && avatarSrc) barAvatar.src = avatarSrc;
                        if (barName) barName.textContent = name;
                        if (barRank) { barRank.textContent = rankText; barRank.style.display = rankText ? '' : 'none'; }
                        if (barNum) barNum.textContent = num;
                        if (barClass) barClass.textContent = cls;
                    })();

                    // Load default tab (小组合作) if no page is loaded yet
                    if (!iframe.src || iframe.src === window.location.href || iframe.src === '') {
                        switchProfileTab('../profile/mygroup.aspx', 'tab-group');
                    }

                    // Show modal
                    modal.classList.remove('hidden');

                    // Trigger entrance animations
                    setTimeout(function() {
                        backdrop.classList.remove('opacity-0');
                        backdrop.classList.add('opacity-100');
                        panel.classList.remove('opacity-0', 'translate-y-4', 'sm:translate-y-0', 'sm:scale-95');
                        panel.classList.add('opacity-100', 'translate-y-0', 'sm:scale-100');
                    }, 10);
                }

                function closeModernGroupModal() {
                    var modal = document.getElementById('modernGroupModal');
                    var backdrop = document.getElementById('modernGroupModalBackdrop');
                    var panel = document.getElementById('modernGroupModalPanel');

                    // Trigger exit animations
                    backdrop.classList.remove('opacity-100');
                    backdrop.classList.add('opacity-0');
                    panel.classList.remove('opacity-100', 'translate-y-0', 'sm:scale-100');
                    panel.classList.add('opacity-0', 'translate-y-4', 'sm:translate-y-0', 'sm:scale-95');

                    // Hide modal after animation completes
                    setTimeout(function() {
                        modal.classList.add('hidden');
                    }, 300);
                }

                // Close modal when clicking the backdrop
                document.getElementById('modernGroupModalBackdrop').addEventListener('click', closeModernGroupModal);

                // Close modal with Escape key
                document.addEventListener('keydown', function(e) {
                    if (e.key === 'Escape' && !document.getElementById('modernGroupModal').classList.contains('hidden')) {
                        closeModernGroupModal();
                    }
                });

                // Legacy Modal popup scripts using TINY.box
                function showPortfolioModal(snum) {
                    var url = "../student/myportfolio.aspx?Snum=" + snum;
                    TINY.box.show({ iframe: url, boxid: 'frameless', width: 800, height: 600, fixed: false, maskopacity: 60, close: true });
                }

                function showWorkModal(wid) {
                    var url = "../student/downwork.aspx?Wid=" + wid;
                    TINY.box.show({ iframe: url, boxid: 'frameless', width: 600, height: 400, fixed: false, maskopacity: 60, close: true });
                }

                // showGroupModal kept for backward compatibility
                function showGroupModal() {
                    openModernGroupModal();
                }

                var i = 2;
                function setbar() {
                    i--;
                    var btnid = window.__myinfoConfig.btnExitId;
                    var btnObj = document.getElementById(btnid);
                    if (!btnObj) return;
                    
                    if (btnObj.value !== "") {
                        btnObj.value = "系统退出";
                    }
                    if (i < 0) {
                        btnObj.disabled = false;
                        if (btnObj.value !== "") {
                            btnObj.value = "系统退出";
                        }
                        return;
                    } else {
                        btnObj.disabled = true;
                    }
                    setTimeout(setbar, 1000);
                }
                setTimeout(setbar, 500);
