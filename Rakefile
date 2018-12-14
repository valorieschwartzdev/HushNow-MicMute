require 'fileutils'

namespace :build do
	task :publish do
		FileUtils.cp_r 'HushNow/bin/Release', 'Release'
		FileUtils.cp_r 'HushNowLauncher/bin/Release', 'Release'
		puts 'Publish successful'
	end
end
